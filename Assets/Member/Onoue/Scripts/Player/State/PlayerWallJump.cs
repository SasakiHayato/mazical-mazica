using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonoState.State;
using MonoState.Data;

public class PlayerWallJump : MonoStateBase
{
    Player _player;
    //Stateが変わる度に呼ばれる
    public override void OnEntry()
    {
        _player.Rigidbody.velocity = Vector2.zero;
        if (_player.Direction.x != 0)
        {
            if (_player.Direction.x < 0)
            {
                //左入力
                _player.Rigidbody.AddForce(Vector2.one * _player.WallJumpPower, ForceMode2D.Impulse);
            }
            else
            {
                //右入力
                _player.Rigidbody.AddForce(new Vector2(-1, 1) * _player.WallJumpPower, ForceMode2D.Impulse);
            }
        }
        _player.IsWallJumped = false;
    }
    //Update
    public override void OnExecute()
    {

    }
    //条件分岐
    public override Enum OnExit()
    {
        if (!_player.FieldTouchOperator.IsTouch(FieldTouchOperator.TouchType.Ground, true))
        {
            if (_player.Rigidbody.velocity.y <= 0)
            {
                return Player.PlayerState.Float;
            }
        }
        if (_player.FieldTouchOperator.IsTouch(FieldTouchOperator.TouchType.Ground))
        {
            return ReturneDefault();
        }
        return Player.PlayerState.WallJump;
    }

    //Awake
    public override void Setup(MonoStateData data)
    {
        _player = data.GetMonoDataUni<Player>(nameof(Player));
    }
}

