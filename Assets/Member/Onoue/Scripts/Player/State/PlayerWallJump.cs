using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonoState.State;
using MonoState.Data;

public class PlayerWallJump : MonoStateBase
{
    Player _player;
    //State���ς��x�ɌĂ΂��
    public override void OnEntry()
    {
        Debug.Log("Entry WallJump");
        _player.Rigidbody.velocity = Vector2.zero;
        if (_player.Direction.x != 0)
        {
            if (_player.Direction.x < 0)
            {
                //������
                //_player.transform.localScale = new Vector3(-1, 1, 1);
                _player.Rigidbody.AddForce(Vector2.one * _player.JumpPower, ForceMode2D.Impulse);
            }
            else
            {
                //�E����
                //_player.transform.localScale = new Vector3(1, 1, 1);
                _player.Rigidbody.AddForce(new Vector2(-1, 1) * _player.JumpPower, ForceMode2D.Impulse);
            }
        }
        _player.IsWallJumped = false;
    }
    //Update
    public override void OnExecute()
    {
        //Debug.Log("Execute PlayerWallJump");
    }
    //��������
    public override Enum OnExit()
    {
        if (_player.FieldTouchOperator.IsTouch(FieldTouchOperator.TouchType.Wall))
        {
            //_player.IsWallJumped = false;
            return ReturneDefault();
        }
        if (!_player.FieldTouchOperator.IsTouch(FieldTouchOperator.TouchType.Ground))
        {
            if (_player.Rigidbody.velocity.y < 0)
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
