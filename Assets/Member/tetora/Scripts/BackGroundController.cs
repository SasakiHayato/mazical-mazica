using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] _backGrounds;
    [SerializeField, Tooltip("Playerの速度にかける係数")]
    float[] _backGroundSpeed;//(例)[0] = 1,[1] = 0.8, [2] = 0.5
    [SerializeField]
    float _spriteSize = 18;

    SpriteRenderer[] _backgroundSpriteClones;
    SpriteRenderer[] _nextBackGrounds;//次に移動する背景
    private void Start()
    {
        InitialSetting();
    }
    private void Update()
    {

    }
    /// <summary>
    /// 背景の初期設定
    /// </summary>
    void InitialSetting()
    {
        Debug.Log("初期設定");
        _backgroundSpriteClones = new SpriteRenderer[_backGrounds.Length];
        CreateOrigin();
        CreateClone();

    }
    void CreateOrigin()
    {
        for (int i = 0; i < _backGrounds.Length; i++)
        {
            var ob = Instantiate(_backGrounds[i], transform);
            ob.transform.position = Vector2.zero;
        }
    }
    void CreateClone()
    {
        //背景の種類だけそれぞれのクローンを作成し、位置をずらしてる
        for (int i = 0; i < _backGrounds.Length; i++)
        {
            _backgroundSpriteClones[i] = Instantiate(_backGrounds[i], transform);
            _backgroundSpriteClones[i].transform.position =
                new Vector2(_spriteSize, _backGrounds[i].transform.position.y);
        }
    }
    void SetBackGroundPos()
    {

    }
}
