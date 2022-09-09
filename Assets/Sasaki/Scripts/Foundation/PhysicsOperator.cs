using UnityEngine;

/// <summary>
/// 物理挙動の操作クラス
/// </summary>

public class PhysicsOperator : MonoBehaviour
{
    [SerializeField] GroundData _groundData;

    float _timer;

    void Awake()
    {
        if (_groundData == null)
        {
            Debug.LogError("GrounDataがありません。");
        }
    }

    void Update()
    {
        
    }
}
