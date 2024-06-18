using System.Collections;
using System.Collections.Generic;
using System.Xml.Resolvers;
using NUnit.Framework.Internal;
using UnityEngine;

public class Player_movement : MonoBehaviour {
    public Rigidbody2D body;
    public LayerMask obstacleMask;

    // Default speed is 1
    public float speed = 1f;

    [Range(0, 10)]
    // Default jump is 3
    public int jumpSpeed = 3;

    [Range(0f, 1f)]
    public float groundDecay = 0.9f;
    public bool grounded;

    public LayerMask groundMask;
    public BoxCollider2D groundCheck;

    float xInput;

    private Animator animator;

    public HudController hudController;

    private float idleTime = 0f;
    private float idleThreshold = 3f;


    // Start is called before the first frame update
    void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        GetInput();
        HandleJump();
    }

    void FixedUpdate() {
        CheckGround();
        MoveWithInput();
        ApplyFriction();
        ExpressionControl();
    }

    private void ExpressionControl() {
        if (hudController != null) {
            if (IsObstacleInDirection(new Vector2(xInput, 0).normalized)) {
                hudController.StartTriggerAnger(1f);
                return;
            }

            if (xInput < 0) {
                hudController.StartLookLeft(1.5f);
                return;
            }
            else if (xInput > 0) {
                hudController.StartLookRight(1.5f);
                return;
            }

            if (!Input.anyKey) {
                hudController.StartRandomBlinking(5f);
            }
        }
    }

    private bool IsObstacleInDirection(Vector2 direction) {
        float raycastDistance = 0.5f; // Temporarily increase the distance for debugging
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance, obstacleMask);
        return (xInput > 0 || xInput < 0) && hit.collider != null;
    }

    void GetInput() {
        xInput = Input.GetAxis("Horizontal");
    }

    void MoveWithInput() {
        if (Mathf.Abs(xInput) > 0.1f) {
            animator.SetBool("isWalking", true);
            body.velocity = new Vector2(xInput * speed, body.velocity.y);

            float direction = Mathf.Sign(xInput);

            if (direction > 0) {
                animator.SetBool("isWalkingRight", true);

            }
            else if (direction < 0) {

                animator.SetBool("isWalkingLeft", true);
            }
        }
        else {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalking", false);
            animator.SetTrigger("idle-trigger");

            if (IsInState("idle")) {
                idleTime += Time.deltaTime;

                if (idleTime >= Random.Range(5f, 10f)) {
                    int randomAnimationNumber = Random.Range(0, 2);
                    if (randomAnimationNumber == 0) {
                        animator.SetTrigger("check-watch-trigger");
                    }
                    else {
                        animator.SetTrigger("smoking-trigger");
                    }
                    idleTime = 0f;
                }
            }

        }
    }

    void HandleJump() {
        if (Input.GetButtonDown("Jump") && grounded) {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    void CheckGround() {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction() {
        if (grounded && xInput == 0 && body.velocity.y == 0) {
            body.velocity *= groundDecay;
        }
    }

    bool IsInState(string stateName) {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 is the layer index
        return stateInfo.IsName(stateName);
    }
}
