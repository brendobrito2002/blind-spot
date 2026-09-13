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

        [Header("AnimaÃ§Ã£o")]
        [SerializeField] private Animator animator;

        private Rigidbody2D rb;
        private Transform currentTarget;

        [Header("Som")]
        [SerializeField] private float soundDistance = 4f;
        private AudioSource audioSource;
        private Transform playerTransform;

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

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 0f;
            }
            audioSource.clip = Resources.Load<AudioClip>("Audio/skeleton-steps");
            audioSource.loop = true;
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

            if (playerTransform != null && audioSource != null)
            {
                float distance = Vector2.Distance(rb.position, playerTransform.position);
                if (distance <= soundDistance)
                {
                    if (!audioSource.isPlaying) audioSource.Play();
                }
                else
                {
                    if (audioSource.isPlaying) audioSource.Pause();
                }
            }
        }

        // Tag "KillZone" no GameObject jÃ¡ resolve a colisÃ£o com o jogador,
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


