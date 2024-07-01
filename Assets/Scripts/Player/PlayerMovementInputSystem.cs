using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInputSystem : MonoBehaviour {
    public Pokus2 pokus;


    public BoxCollider2D groundCheck;
    public LayerMask groundLayer;
    public LayerMask obstacleMask;
    [Range(0, 10)][SerializeField] const float shortJumpForce = 3f; //3f
    [Range(0, 10)][SerializeField] const float longJumpForce = 4f;

    const float speed = 1f;
    Vector2 lastPosition;
    Rigidbody2D rigidBody;
    bool isHoldingJump = false;
    float jumpHoldTime = 0f;
    const float maxJumpHoldTime = 0.3f;
    bool isGrounded;
    bool isFalling;

    ////////////////// AGRO/PASSIVE ////////////////////
    public enum AgroState {
        Passive,
        Agro
    }
    AgroState agroState = AgroState.Passive;

    GameObject playerAimWeapon;

    //////////////////// EVENTS ////////////////////
    public delegate void WalkingDirection(float walkingDirection);
    public static event WalkingDirection OnWalkEvent;


    public delegate void Falling(bool isFalling);
    public static event Falling FallingEvent;


    public delegate void Emotions(float duration);
    public static event Emotions Angry;

    public delegate void DrawOrHolsterWeaponDelegate();
    public static event DrawOrHolsterWeaponDelegate DrawWeaponEvent;
    public static event DrawOrHolsterWeaponDelegate HolsterWeaponEvent;
    //public static event Emotions Terrified;


    ///////////////// UNITY METHODS /////////////////
    void Awake() {
        rigidBody = rigidBody != null ? rigidBody : GetComponent<Rigidbody2D>();
        playerAimWeapon = GameObject.FindWithTag("AimController");
    }

    void Update() {
        if (isHoldingJump) {
            jumpHoldTime += Time.deltaTime;
        }
    }

    void FixedUpdate() {
        CheckGround();
        CheckAndInvokeFallingEvent();
        CheckAgroState();

        if (IsObstacleInDirection(lastPosition)) {
            Angry.Invoke(1.5f);
        }
    }

    public void DrawOrHolsterWeapon() {
        if (agroState == AgroState.Agro) {
            agroState = AgroState.Passive;
            HolsterWeaponEvent.Invoke();
        } else {
            agroState = AgroState.Agro;
            DrawWeaponEvent.Invoke();
        }
        pokus.ChangeLegs();
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
        } else if (ctx.canceled && isGrounded) {
            isHoldingJump = false;
            if (jumpHoldTime >= maxJumpHoldTime) {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, longJumpForce);
            } else {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, shortJumpForce);
            }
        }
    }

    void CheckGround() {
        isGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer).Length > 0;
    }

    void CheckAgroState() {
        if (agroState == AgroState.Agro) {
            playerAimWeapon.SetActive(true);
        } else {
            playerAimWeapon.SetActive(false);
        }
    }

    void CheckAndInvokeFallingEvent() {
        if (!isGrounded && rigidBody.velocity.y < 0) {
            isFalling = true;
            FallingEvent.Invoke(isFalling);
        } else {
            isFalling = false;
            FallingEvent.Invoke(isFalling);
        }
    }

    bool IsObstacleInDirection(Vector2 direction) {
        float raycastDistance = 0.3f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, obstacleMask);
        return (direction.x > 0 || direction.x < 0) && hit.collider != null;
    }
}
