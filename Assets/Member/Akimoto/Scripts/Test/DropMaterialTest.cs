using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class DropMaterialTest : MonoBehaviour
{
    [SerializeField] FusionMaterialObject _prefab;
    [SerializeField] Player _player;
    [SerializeField] RawMaterialData _rawMaterialData;

    private void Start()
    {
        Instantiate(_player, new Vector2(20, 20), Quaternion.identity);
        FusionMaterialObject obj = FusionMaterialObject.Init(_prefab, Vector2.zero, _rawMaterialData.GetMaterialData(RawMaterialID.BombBean), _player);
    }
}
