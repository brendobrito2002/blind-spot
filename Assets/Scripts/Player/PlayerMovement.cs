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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {

        if (isDead) return;

        animator.SetBool("IsWalking", true);

        if(context.canceled)
        {
            animator.SetBool("IsWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
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
}
