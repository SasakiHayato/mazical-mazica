using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonoState.State;
using MonoState.Data;
public class PlayerRun : MonoStateBase
{
    Player _player;
    //Stateが変わる度に呼ばれる
    public override void OnEntry()
    {
        //Debug.Log("Entry PlayerRun");
    }
    //Update
    public override void OnExecute()
    {
        Vector2 dir = _player.Direction;
        Vector2 velocity = new Vector2(dir.x * _player.Speed, _player.Rigidbody.velocity.y);
        _player.Rigidbody.velocity = velocity;
    }
    //条件分岐
    public override Enum OnExit()
    {
        if (_player.IsJumped)
        {
            return Player.PlayerState.Jump;
        }
        if (_player.IsWallJumped)
        {
            return Player.PlayerState.WallJump;
        }
        if (_player.Direction == Vector2.zero)
        {
            return Player.PlayerState.Idle;
        }
        if (!_player.FieldTouchOperator.IsTouch(FieldTouchOperator.TouchType.Ground))
        {
            if (_player.Rigidbody.velocity.y < 0)
            {
                return Player.PlayerState.Float;
            }
        }
        return Player.PlayerState.Run;
    }

    //Awake
    public override void Setup(MonoStateData data)
    {
        _player = data.GetMonoDataUni<Player>(nameof(Player));
    }
}

