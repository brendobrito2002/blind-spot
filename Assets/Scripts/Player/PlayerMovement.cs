using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public bool isDead = false;
    public bool IsUsingAbility { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetFloat("LastInputX", 0f);
        animator.SetFloat("LastInputY", -1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || IsUsingAbility)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isDead) return;

        moveInput = context.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);

            Vector2 normalized = moveInput.normalized;
            animator.SetFloat("InputX", normalized.x);
            animator.SetFloat("InputY", normalized.y);

            if (!IsUsingAbility)
            {
                animator.SetFloat("LastInputX", normalized.x);
                animator.SetFloat("LastInputY", normalized.y);
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!isDead && other.CompareTag("KillZone"))
        {
            isDead = true;

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);

            animator.SetBool("IsWalking", false);
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("Dead");
        }
    }

    public void OnDeathAnimation()
    {
        if (isDead)
        {
            isDead = false;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        }
    }

    public void OnSeismicRadarAnimation()
    {
        SetUsingAbility(true);
        animator.SetBool("IsRadar", true);

        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    public void OnRadarAnimationEnd()
    {
        animator.SetBool("IsRadar", false);
        SetUsingAbility(false);
    }

    public void SetUsingAbility(bool value)
    {
        IsUsingAbility = value;
    }
}
