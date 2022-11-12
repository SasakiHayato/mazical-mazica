using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonoState.State;

public class PlayerAttack : MonoStateAttribute
{
    //Update
    public override void Execute()
    {
        Debug.Log("Execute PlayerAttack");
    }
    //条件分岐
    public override Enum Exit()
    {
        return Player.PlayerState.Attack;
    }
    //Stateが変わる度に呼ばれる
    public override void OnEnable()
    {
        Debug.Log("Enable PlayerAttack");
    }
    //Awake
    public override void Setup()
    {
        Debug.Log("Setup PlayerAttack");
    }
}

