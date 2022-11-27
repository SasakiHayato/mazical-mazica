using UnityEngine;
using ObjectPool;
using ObjectPool.Event;

public interface IEffectExecutable
{
    /// <summary>
    /// Setup
    /// </summary>
    /// <param name="particle">対象Effect</param>
    void SetEffect(ParticleSystem particle);
    /// <summary>
    /// Update
    /// </summary>
    /// <returns>終了時Trueを返す</returns>
    bool Execute();
    /// <summary>
    /// 呼ばれるたびに初期化
    /// </summary>
    void Initalize();
}

public class Effect : MonoBehaviour, IPool, IPoolOnEnableEvent
{
    [SerializeReference, SubclassSelector]
    IEffectExecutable _executable;

    ParticleSystem _particle;

    void IPool.Setup(Transform parent)
    {
        _particle = GetComponent<ParticleSystem>();
        _executable.SetEffect(_particle);
    }

    void IPoolOnEnableEvent.OnEnableEvent()
    {
        _executable.Initalize();
        _particle.Play();
    }

    bool IPool.Execute()
    {
        return _executable.Execute();
    }
}
