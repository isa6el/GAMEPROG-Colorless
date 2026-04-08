using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float jumpForce = 200f;

    [Header("Animation")]
    private Animator animator;

    [Header("Ground Settings")]
    [SerializeField]
    // What layers are considered as ground
    private LayerMask groundLayers;

    [SerializeField]
    // A reference to where the ground check happens. It's usually the feet of the player
    private Transform groundPoint;

    [SerializeField]
    // How big is the radius of the groundchecker
    private float groundRadius = 0.1f;

    [SerializeField]
    private Transform spawnPoint;

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

    /*    private void Start()
        {
            GameManager.Instance.OnGameStarted += HandleGameStarted;
            GameManager.Instance.OnGameEnded += HandleGameEnded;
        }*/

    private void HandleGameStarted()
    {
        canControl = true;
        playerRigidbody.WakeUp();
        transform.position = spawnPoint.position;
    }

    private void HandleGameEnded(bool isWin)
    {
        canControl = false;
        playerRigidbody.Sleep();
    }

    // Check input in the update function
    private void Update()
    {
        if (!canControl)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        float animationMoveSpeed = Mathf.Abs(horizontalInput);
        animator.SetFloat("moveSpeed", animationMoveSpeed);

        // ADDED: Tell the animator if we are on the ground
        animator.SetBool("isGrounded", IsGrounded());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpInput = true;
        }

        Flip(horizontalInput);
    }

    // Process the input in FixedUpdate for all physics calculation
    private void FixedUpdate()
    {
        float horizontalMovement = horizontalInput * moveSpeed;
        playerRigidbody.linearVelocity = new Vector2(horizontalMovement, playerRigidbody.linearVelocityY);

        // Allow jumping only if we are grounded and there is jump input to be processed
        if (IsGrounded() && isJumpInput)
        {
            // Add a vertical force
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // Mark the jump input as processed
            isJumpInput = false;
        }

        if (!IsGrounded() && isJumpInput)
        {
            isJumpInput = false;
        }
    }

    private bool IsGrounded()
    {
        // Spawn a circle collider with the ground radius
        // Check if the circle hits any ground layers
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundPoint.position, groundRadius, groundLayers);

        // A safety check: make sure that we ignore own gameObject collider
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
        // If we are moving to the right, we will only flip if we're facing left
        // If we are moving to the left, flip only when facing right
        if (movement > 0 && !isFacingRight ||
            movement < 0 && isFacingRight)
        {
            // Flip our value
            isFacingRight = !isFacingRight;
            // You can optionally change the x scale of your player
            // or change the flip value of the sprite
            spriteRenderer.flipX = !isFacingRight;
        }
    }
}
