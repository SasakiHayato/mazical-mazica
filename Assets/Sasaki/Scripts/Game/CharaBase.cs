using UnityEngine;
using UniRx;
using CustomPhysics;

/// <summary>
/// Characterの基底クラス
/// </summary>

[RequireComponent(typeof(PhysicsOperator))]
public abstract class CharaBase : MonoBehaviour
{
    [SerializeField] StatusData _statusData;

    protected PhysicsOperator PhysicsOperator { get; private set; } 

    void Start()
    {
        _statusData.Initalize();
        SubScribe();

        PhysicsOperator = GetComponent<PhysicsOperator>();

        Setup();
    }

    void SubScribe()
    {
        _statusData.ObservableHP
            .Select(hp => hp <= 0)
            .Subscribe(_ => DeadEvent())
            .AddTo(this);
    }

    /// <summary>
    /// 初期化. Start関数
    /// </summary>
    protected abstract void Setup();

    /// <summary>
    /// 死んだ際の処理
    /// </summary>
    protected virtual void DeadEvent()
    {
        Destroy(gameObject);
    }
}
