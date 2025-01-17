using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonoState.State;
using MonoState.Data;
using MonoState.Opration;

public class PlayerFloat : MonoStateBase, IStateExitEventable
{
    Player _player;
    AnimOperator _anim;
    PlayerStateData _playerStateData;
    float _acceleration;

    readonly float LimitAcceleration = 0.35f;

    void IStateExitEventable.ExitEvent()
    {
        _playerStateData.Rigid.MaxAcceleration = _acceleration;
    }
    //Stateが変わる度に呼ばれる
    public override void OnEntry()
    {
        _anim.OnPlay("Fall");
        _acceleration = _playerStateData.Rigid.MaxAcceleration;
        _playerStateData.Rigid.MaxAcceleration = LimitAcceleration;
        _playerStateData.Rigid.SetMoveDirection = Vector2.zero;
    }
    //Update
    public override void OnExecute()
    {
        Vector2 dir = _playerStateData.ReadMoveDirection;
        _playerStateData.Rigid.SetMoveDirection = new Vector2(dir.x * _playerStateData.Status.Speed, _playerStateData.Rigid.ReadVelocity.y);
    }
    //条件分岐
    public override Enum OnExit()
    {
        if (_player.FieldTouchOperator.IsTouch(FieldTouchOperator.TouchType.Wall, true)
            /*&& MathF.Abs(_playerStateData.Rigid.ReadVelocity.x) <= 0*/)
        {
            _playerStateData.Rigid.ResetImpalse();
            Vector2 moveDirection = _playerStateData.Rigid.ReadVelocity;
            moveDirection.x = 0;
            _playerStateData.Rigid.SetMoveDirection = moveDirection;

            return Player.PlayerState.IsStick;
        }
        if (_player.FieldTouchOperator.IsTouch(FieldTouchOperator.TouchType.Ground, true))
        {
            return ReturneState();
        }
        return Player.PlayerState.Float;
    }

    //Awake
    public override void Setup(MonoStateData data)
    {
        _player = data.GetMonoDataUni<Player>(nameof(Player));
        _anim = data.GetMonoDataUni<AnimOperator>(nameof(AnimOperator));
        _playerStateData = data.GetMonoData<PlayerStateData>(nameof(PlayerStateData));
    }
}

