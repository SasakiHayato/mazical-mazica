using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
using ObjectPool;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Sounder _sounderPrefab;
    [SerializeField] int _poolCount;
    [SerializeField] List<SoundDataAsset> _soundDataAssetList;
    [SerializeField, Range(0, 1)] float _masterVolume;
    [SerializeField, Range(0, 1)] float _bgmVolume;
    [SerializeField, Range(0, 1)] float _seVolume;

    Pool<Sounder> _pool = new Pool<Sounder>();

    static SoundManager Instance = null;

    void Awake()
    {
        Instance = this;

        _pool
            .SetMono(_sounderPrefab, _poolCount)
            .IsSetParent(transform)
            .CreateRequest();
    }

    void Update()
    {
        SoundMasterData.MasterValume = _masterVolume;
        SoundMasterData.BGMVolume = _bgmVolume;
        SoundMasterData.SEVoume = _seVolume;
    }

    public static void PlayRequest(SoundType type, string path)
    {
        SoundDataAsset asset = Instance._soundDataAssetList.Find(s => s.SoundType == type);
        SoundDataAsset.SoundData data = asset.GetSoundData(path);

        if (data == null)
        {
            Debug.Log("サウンドデータが存在しません");
            return;
        }

        Sounder sounder = Instance._pool.UseRequest(out System.Action action);
        action += () => sounder.SetData(data, asset.VolumeType);

        action.Invoke();
    }
}
