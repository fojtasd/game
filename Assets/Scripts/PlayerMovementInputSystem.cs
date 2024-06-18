using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Resolvers;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInputSystem : MonoBehaviour {
    private Rigidbody2D rigidBody;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask obstacleMask;



    public delegate void Emotions();
    public static event Emotions Angry;
    public static event Emotions Terrified;


    public delegate void WalkingDirection(float walkingDirection);
    public static event WalkingDirection OnWalk;

    private float speed = 1f;
    [Range(0, 10)][SerializeField] private float jumpingPower = 3f;
    private bool isGrounded;


    void Awake() {
        rigidBody ??= GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        CheckGround();
    }

    public void Move(InputAction.CallbackContext ctx) {
        if (IsObstacleInDirection(ctx)) {
            Angry.Invoke();
        }
        rigidBody.velocity = ctx.ReadValue<Vector2>() * speed;
        OnWalk.Invoke(ctx.ReadValue<Vector2>().x);
    }

    public void Jump(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            if (isGrounded) {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
            }
        }
    }

    private void CheckGround() {
        isGrounded = !Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsObstacleInDirection(InputAction.CallbackContext ctx) {
        float raycastDistance = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, ctx.ReadValue<Vector2>(), raycastDistance, obstacleMask);
        return (ctx.ReadValue<Vector2>().x > 0 || ctx.ReadValue<Vector2>().y < 0) && hit.collider != null;
    }


}
