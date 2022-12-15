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
/// 爆発x火力<br/>
/// 真っすぐ飛んでいき、何かに接触すると爆発して消える
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
/// 爆発x爆発<br/>
/// 放物線を描いて飛んでいき、何かに接触すると爆発して消える
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
/// <summary>
/// 爆発x貫通<br/>
/// 真っすぐ飛んでいき、敵に接触すると爆発する。一定回数敵に接触するか壁や地面に当たると消滅
/// </summary>
public class BlastPenetration : IFusionBullet
{
    [SerializeField] Blast _blastPrefab;
    [SerializeField] float _blastRange;
    [SerializeField] float _blastDuraion;
    [SerializeField] int _destroyHitNum;
    /// <summary>敵と接触した回数</summary>
    //[SerializeField] int _hitCount;
    public int Damage { private get; set; }
    public ObjectType ObjectType { private get; set; }

    public void Dispose() { }

    public void Hit(IDamagable damageble, Vector2 position)
    {
        damageble.AddDamage(Damage);
        Blast.Init(_blastPrefab, position, _blastRange, _blastDuraion, Damage, ObjectType);
    }

    public void Idle() { }

    public bool IsDestroy(Collider2D collision)
    {
        return true;
        //if (collision.TryGetComponent(out IDamagable damageble) && damageble.ObjectType == ObjectType.Enemy)
        //{
        //    Debug.Log($"接触回数:{_hitCount} 設定回数:{_destroyHitNum}");
        //    if (_hitCount >= _destroyHitNum)
        //    {
        //        Debug.Log("HitCountが上限に達した");
        //        return true;
        //    }
        //    else
        //    {
        //        _hitCount++;
        //        return false;
        //    }
        //}
        //else
        //{
        //    Debug.Log("Damageble以外に接触した");
        //    return true;
        //}
    }
}
