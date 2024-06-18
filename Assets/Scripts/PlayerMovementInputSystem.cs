using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInputSystem : MonoBehaviour {
    private Rigidbody2D rigidBody;
    public BoxCollider2D groundCheck;
    public LayerMask groundLayer;
    public LayerMask obstacleMask;
    private const float speed = 1f;
    private Vector2 lastPosition;
    [Range(0, 10)][SerializeField] private const float shortJumpForce = 3f; //3f
    [Range(0, 10)][SerializeField] private const float longJumpForce = 4f;
    private bool isHoldingJump = false;
    private float jumpHoldTime = 0f;
    public const float maxJumpHoldTime = 0.3f;
    private bool isGrounded;
    private bool isFalling;

    //////////////////// EVENTS ////////////////////
    public delegate void WalkingDirection(float walkingDirection);
    public static event WalkingDirection OnWalkEvent;


    public delegate void Falling(bool isFalling);
    public static event Falling FallingEvent;


    public delegate void Emotions(float duration);
    public static event Emotions Angry;
    //public static event Emotions Terrified;


    ///////////////// UNITY METHODS /////////////////
    void Awake() {
        rigidBody = rigidBody != null ? rigidBody : GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (isHoldingJump) {
            jumpHoldTime += Time.deltaTime;
        }
    }

    void FixedUpdate() {
        CheckGround();
        CheckAndInvokeFallingEvent();

        if (IsObstacleInDirection(lastPosition)) {
            Angry.Invoke(1.5f);
        }
    }

    public void Move(InputAction.CallbackContext ctx) {
        rigidBody.velocity = ctx.ReadValue<Vector2>() * speed;
        lastPosition = ctx.ReadValue<Vector2>() * speed;
        OnWalkEvent.Invoke(ctx.ReadValue<Vector2>().x);
    }

    public void Jump(InputAction.CallbackContext ctx) {
        if (ctx.started && isGrounded) {
            isHoldingJump = true;
            jumpHoldTime = 0f;
        }
        else if (ctx.canceled && isGrounded) {
            isHoldingJump = false;
            if (jumpHoldTime >= maxJumpHoldTime) {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, longJumpForce);
            }
            else {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, shortJumpForce);
            }
        }
    }

    private void CheckGround() {
        isGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer).Length > 0;
    }

    private void CheckAndInvokeFallingEvent() {
        if (!isGrounded && rigidBody.velocity.y < 0) {
            isFalling = true;
            FallingEvent.Invoke(isFalling);
        }
        else {
            isFalling = false;
            FallingEvent.Invoke(isFalling);
        }
    }

    private bool IsObstacleInDirection(Vector2 direction) {
        float raycastDistance = 0.3f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, obstacleMask);
        return (direction.x > 0 || direction.x < 0) && hit.collider != null;
    }
}
