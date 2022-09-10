using UnityEngine;
using CustomPhysics.Data;

namespace CustomPhysics
{
    /// <summary>
    /// 物理挙動の操作クラス
    /// </summary>

    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsOperator : MonoBehaviour
    {
        [SerializeField] GroundData _groundData;
        [SerializeField] GravityData _gravityData;

        Rigidbody2D _rb;

        public bool IsGround { get; private set; }

        void Awake()
        {
            if (_groundData == null)
            {
                Debug.LogError("GrounDataがありません。");
            }

            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;

            _groundData.SetUp(transform);
        }

        void Update()
        {
            IsGround = _groundData.IsGround;
        }

        public void Move(Vector2 velocity, bool attributeGravity = true)
        {
            if (attributeGravity)
            {
                velocity.y += _gravityData.Gravity;
            }

            _rb.velocity = velocity;
        }
    }
}