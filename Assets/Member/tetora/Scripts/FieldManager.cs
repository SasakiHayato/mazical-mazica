using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class FieldManager : MonoBehaviour, IGameSetupable, IFieldEffectable<float>
{
    [SerializeField] float _hitStopTime;
    [SerializeField] List<InitialMaterialNum> _initialMaterialNum;
    [SerializeField] CharacterManager _characterManager;

    [SerializeField] MapCreaterBase _createMap;
    int _hierarchyNum;
    Subject<List<RawMaterialID>> _materialIDSubject = new Subject<List<RawMaterialID>>();
    public int HierarchyNum { get => _hierarchyNum; set => _hierarchyNum = value; }
    /// <summary>このステージで使う素材IDのListを発行するSubject</summary>
    public IObservable<List<RawMaterialID>> MaterialList => _materialIDSubject;

    int IGameSetupable.Priority => 3;

    FieldEffect.EffectType IFieldEffectDatable.EffectType => FieldEffect.EffectType.HitStop;

    public static float DefaultHitStopTime { get; private set; }
    public static float BulletHitStopTime { get; private set; }

    void Awake()
    {
        GameController.Instance.AddGameSetupable(this);
    }

    void Start()
    {
        EffectStocker.Instance.AddFieldEffect(this);
    }

    public void Setup()
    {
        // _characterManager.Setup();
        // _createMap.InitialSet();
        // _characterManager.CreatePlayer(_createMap.PlayerTransform);
    }

    void IGameSetupable.GameSetup()
    {
        DefaultHitStopTime = _hitStopTime;
        BulletHitStopTime = _hitStopTime / 4;

        //今ステージで登場する素材たち
        //ステージデータを作る際はここも変更すること
        List<RawMaterialID> defMaterials = new List<RawMaterialID>()
        {
            RawMaterialID.BombBean,
            RawMaterialID.PowerPlant,
            RawMaterialID.Penetration,
            RawMaterialID.Poison
        };
        _materialIDSubject.OnNext(defMaterials);

        //プレイヤー生成時に素材を持たせる
        _characterManager.PlayerSpawn.Subscribe(p =>
        {
            defMaterials.ForEach(m =>
            {
                foreach (var initm in _initialMaterialNum)
                {
                    if (initm.MaterialID == m)
                    {
                        p.Storage.AddMaterial(m, initm.Num);
                        break;
                    }
                }
            });
        })
        .AddTo(_characterManager);

        //プレイヤーを生成
        _characterManager.CreatePlayer(_createMap.PlayerTransform);
    }

    /// <summary>死亡判定</summary>
    /// <param name="type">死亡したキャラのType</param>
    void OnGameEndJudge(CharaType type)
    {
        switch (type)
        {
            case CharaType.Player:
                GameOver();
                break;
            case CharaType.Boss:
                GameClear();
                break;
            case CharaType.Mob:
                //DeadMob();
                break;
        }
    }
    /// <summary>GameOver処理</summary>
    void GameOver()
    {

    }
    /// <summary>GameClear処理</summary>
    void GameClear()
    {

    }

    void IFieldEffectable<float>.Execute(float value)
    {
        StartCoroutine(OnHitStop(value));
    }

    IEnumerator OnHitStop(float timer)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(timer);
        Time.timeScale = 1;
    }

    /// <summary>
    /// プレイヤーの初期素材数
    /// </summary>
    [Serializable]
    public class InitialMaterialNum
    {
        [SerializeField] RawMaterialID _materialID;
        [SerializeField] int _num;
        /// <summary>素材ID</summary>
        public RawMaterialID MaterialID => _materialID;
        /// <summary>所持素材数</summary>
        public int Num => _num;
    }

    /// <summary>Mobが死んだときの処理</summary> //Note. もしかしたらいらない
    //void DeadMob()
    //{

    //}
}
public enum CharaType
{
    Player, Boss, Mob
}
