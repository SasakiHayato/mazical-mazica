using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] CharacterManager _characterManager;
    //[SerializeField] Transform _playerSpawnPosition;
    [SerializeField] CreateMap _createMap;
    int _hierarchyNum;
    public int HierarchyNum { get => _hierarchyNum; set => _hierarchyNum = value; }
    private void Start()
    {
        Setup();
    }
    public void Setup()
    {
        _characterManager.Setup();
        _createMap.InitialSet();
        _characterManager.CreatePlayer(_createMap.PlayerTransform);
        //_characterManager.CreatePlayer(_playerSpawnPosition); 秋元作
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
                DeadMob();
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
    /// <summary>Mobが死んだときの処理</summary>
    void DeadMob()
    {

    }
}
public enum CharaType
{
    Player, Boss, Mob
}
