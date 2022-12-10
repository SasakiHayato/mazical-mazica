using UnityEngine;
using ObjectPool;
using ObjectPool.Event;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public interface IEffectExecutable
{
    /// <summary>
    /// Setup
    /// </summary>
    /// <param name="particle">対象Effect</param>
    void SetEffect(GameObject effect, ParticleSystem particle);
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
    List<IEffectExecutable> _executableList;

    int _endCount = 0;

    ParticleSystem _particle;

    void IPool.Setup(Transform parent)
    {
        _particle = GetComponent<ParticleSystem>();
        _executableList.ForEach(e => e.SetEffect(gameObject, _particle));
    }

    void IPoolOnEnableEvent.OnEnableEvent()
    {
        _endCount = 0;
        _executableList.ForEach(e => e.Initalize());

        if (_particle != null)
        {
            _particle.Play();
        }

        foreach (IEffectExecutable effectable in _executableList)
        {
            StartCoroutine(OnProcess(effectable.Execute));
        }
    }

    bool IPool.Execute()
    {
        return _endCount >= _executableList.Count;
    }

    IEnumerator OnProcess(Func<bool> func)
    {
        while (!func.Invoke())
        {
            yield return null;
        }

        _endCount++;
    }
}
