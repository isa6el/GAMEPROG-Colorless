using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 200f;

    [Header("Animation")]
    private Animator animator;

    [Header("Ground Settings")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private float groundRadius = 0.1f;

    [SerializeField] private Transform spawnPoint;

    private Rigidbody2D playerRigidbody;
    private SpriteRenderer spriteRenderer;
    private float horizontalInput;
    private bool isFacingRight = true;
    private bool isJumpInput = false;
    private bool canControl = true;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!canControl) return;

        horizontalInput = Input.GetAxis("Horizontal");

        float animationMoveSpeed = Mathf.Abs(horizontalInput);
        animator.SetFloat("moveSpeed", animationMoveSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpInput = true;
        }

        Flip(horizontalInput);
    }

    private void FixedUpdate()
    {
        float horizontalMovement = horizontalInput * moveSpeed;
        playerRigidbody.linearVelocity = new Vector2(horizontalMovement, playerRigidbody.linearVelocityY);

        if (IsGrounded() && isJumpInput)
        {
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            isJumpInput = false;
        }

        if (!IsGrounded() && isJumpInput)
        {
            isJumpInput = false;
        }
    }

    private bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundPoint.position, groundRadius, groundLayers);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != this.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void Flip(float movement)
    {
        if (movement > 0 && !isFacingRight || movement < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !isFacingRight;
        }
    }
}
