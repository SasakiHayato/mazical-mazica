using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの制御クラス
/// </summary>

public class Enemy : CharaBase
{
    protected override void Setup()
    {
        
    }

    void Update()
    {
        
        PhysicsOperator.Move(Vector2.zero);
    }
}
