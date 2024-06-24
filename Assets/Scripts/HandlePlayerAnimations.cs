using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandlePlayerAnimations : MonoBehaviour {
    private Animator animator;
    private float idleTime = 0f;
    private bool isFalling;
    private float noWalkingTime = 0f;
    private float secondsAfterIdleIsSet = 10f;

    //////////////////// EVENTS ////////////////////
    public delegate void EyesDirection(float duration = 0.5f);
    public static event EyesDirection OnLookLeft;
    public static event EyesDirection OnLookRight;
    public static event EyesDirection OnLookUp;
    public static event EyesDirection OnLookDown;

    ///////////////// UNITY METHODS /////////////////
    void Start() {
        animator = GetComponent<Animator>();
        StartCoroutine(MeasureIdleTime());
    }

    void Update() {
        SetIdleIfNotWalkingForAmountOfTime();
    }

    void OnEnable() {
        PlayerMovementInputSystem.OnWalkEvent += AdjustWalkAnimation;
        PlayerMovementInputSystem.FallingEvent += CheckIfFalling;
    }

    void OnDisable() {
        PlayerMovementInputSystem.OnWalkEvent -= AdjustWalkAnimation;
        PlayerMovementInputSystem.FallingEvent -= CheckIfFalling;
    }

    private void AdjustWalkAnimation(float walkingDirection = 0f) {
        if (walkingDirection > 0) {
            SetWalkingRight();
        }
        else if (walkingDirection < 0) {
            SetWalkingLeft();
        }
        else {
            SetNoWalking();
        }
    }

    private void CheckIfFalling(bool isFalling) {
        this.isFalling = isFalling;
    }

    private void SetWalkingLeft() {
        OnLookLeft.Invoke(1f);
        animator.SetBool("isWalking", true);
        animator.SetBool("isWalkingRight", false);
        animator.SetBool("isWalkingLeft", true);
    }

    private void SetWalkingRight() {
        OnLookRight.Invoke(1f);
        animator.SetBool("isWalking", true);
        animator.SetBool("isWalkingLeft", false);
        animator.SetBool("isWalkingRight", true);
    }

    private void SetIdle() {
        animator.SetTrigger("idle-trigger");
    }

    private void SetNoWalking() {
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalkingRight", false);
        animator.SetBool("isWalkingLeft", false);
    }

    private IEnumerator MeasureIdleTime() {
        while (true) {
            while (IsIdleAnimationPlaying() || isFalling) {
                yield return new WaitForSeconds(1f);
                idleTime = 0f;
            }
            yield return new WaitForSeconds(1f);
            idleTime += 1f;

            if (IsInState("idle")) {
                if (idleTime >= 5f) {
                    int randomAnimationNumber = Random.Range(0, 2);
                    if (randomAnimationNumber == 0) {
                        animator.SetTrigger("check-watch-trigger");
                    }
                    else {
                        animator.SetTrigger("smoking-trigger");
                    }
                }
            }
        }
    }

    private void SetIdleIfNotWalkingForAmountOfTime() {
        if (IsInState("facing-right") || IsInState("facing-left")) {
            noWalkingTime += Time.deltaTime;
            if (noWalkingTime >= secondsAfterIdleIsSet) {
                animator.SetTrigger("idle-trigger");
                noWalkingTime = 0f;
            }
        }
    }

    bool IsInState(string stateName) {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }

    private bool IsIdleAnimationPlaying() {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length > stateInfo.normalizedTime;
    }



    //////////////////////////// EYES DIRECTION EVENTS ////////////////////////////
    public void LookLeft() {
        OnLookLeft.Invoke();
    }

    public void LookRight() {
        OnLookRight.Invoke();
    }

    public void LookUp() {
        OnLookUp.Invoke();
    }

    public void LookDown() {
        OnLookDown.Invoke();
    }
}
