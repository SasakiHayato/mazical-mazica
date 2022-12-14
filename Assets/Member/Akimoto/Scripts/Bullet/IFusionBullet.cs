using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public interface IFusionBullet
{
    /// <summary>
    /// 破棄可能かどうか
    /// </summary>
    /// <returns></returns>
    public bool IsDestroy(Collider2D collision);
    /// <summary>
    /// 破棄時
    /// </summary>
    public void Dispose();
    /// <summary>
    /// 接触時の挙動
    /// </summary>
    public void Hit(IDamagable damageble, Vector2 position);
    /// <summary>
    /// 通常時の挙動
    /// </summary>
    public void Idle();
    /// <summary>
    /// 与えるダメージ
    /// </summary>
    public int Damage { set; }
    /// <summary>
    /// 誰から撃たれた弾か
    /// </summary>
    public ObjectType ObjectType { set; }
}

/// <summary>
/// 爆発x火力
/// </summary>
public class BlastPower : IFusionBullet
{
    [SerializeField] Blast _blastPrefab;
    [SerializeField] float _blastRange;
    [SerializeField] float _blastDuraion;
    public int Damage { private get; set; }
    public ObjectType ObjectType { private get; set; }

    public void Idle() { }

    public void Hit(IDamagable damageble, Vector2 position)
    {
        damageble.AddDamage(Damage);
        Blast.Init(_blastPrefab, position, _blastRange, _blastDuraion, Damage, ObjectType);
    }

    public void Dispose() { }

    public bool IsDestroy(Collider2D collision) => true;
}

/// <summary>
/// 爆発x爆発
/// </summary>
public class BlastBlast : IFusionBullet
{
    [SerializeField] Blast _blastPrefab;
    [SerializeField] float _blastRange;
    [SerializeField] float _blastDuraion;
    public int Damage { private get; set; }
    public ObjectType ObjectType { private get; set; }

    public void Dispose() { }

    public void Hit(IDamagable damageble, Vector2 position)
    {
        damageble.AddDamage(Damage);
        Blast.Init(_blastPrefab, position, _blastRange, _blastDuraion, Damage, ObjectType);
    }

    public void Idle() { }

    public bool IsDestroy(Collider2D collision) => true;
}