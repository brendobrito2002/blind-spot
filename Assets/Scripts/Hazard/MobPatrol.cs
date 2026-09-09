using UnityEngine;

namespace BlindSpot.Hazards
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MobPatrol : MonoBehaviour
    {
        [Header("Pontos da Patrulha")]
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;

        [Header("Movimento")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float reachThreshold = 0.1f;

        [Header("Animação")]
        [SerializeField] private Animator animator;

        private Rigidbody2D rb;
        private Transform currentTarget;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            if (pointA == null || pointB == null)
            {
                Debug.LogWarning($"{name}: MobPatrolSimple precisa de pointA e pointB.", this);
                enabled = false;
                return;
            }

            rb.position = pointA.position;
            currentTarget = pointB;
        }

        private void FixedUpdate()
        {
            Vector2 toTarget = (Vector2)currentTarget.position - rb.position;

            if (toTarget.magnitude <= reachThreshold)
            {
                currentTarget = (currentTarget == pointA) ? pointB : pointA;
                return;
            }

            Vector2 direction = toTarget.normalized;
            Vector2 step = direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + step);

            if (animator != null)
            {
                animator.SetFloat("InputX", direction.x);
                animator.SetFloat("InputY", direction.y);
            }
        }

        // Tag "KillZone" no GameObject já resolve a colisão com o jogador,
        // igual aos outros hazards.

        private void OnDrawGizmos()
        {
            if (pointA == null || pointB == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.08f);
            Gizmos.DrawSphere(pointB.position, 0.08f);
        }
    }
}