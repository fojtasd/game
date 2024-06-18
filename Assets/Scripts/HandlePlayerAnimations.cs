using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandlePlayerAnimations : MonoBehaviour {
    private Animator animator;
    private float idleTime = 0f;

    void Start() {
        animator = GetComponent<Animator>();
        StartCoroutine(MeasureIdleTime());
    }

    void OnEnable() {
        PlayerMovementInputSystem.OnWalk += AdjustWalkAnimation;
    }

    void OnDisable() {
        PlayerMovementInputSystem.OnWalk -= AdjustWalkAnimation;
    }

    private void AdjustWalkAnimation(float walkingDirection = 0) {
        if (walkingDirection > 0) {
            SetWalkingRight();
        }
        else if (walkingDirection < 0) {
            SetWalkingLeft();
        }
        else if (walkingDirection == 0) {
            SetIdle();
        }
    }

    private void SetWalkingLeft() {
        animator.SetBool("isWalking", true);
        animator.SetBool("isWalkingRight", false);
        animator.SetBool("isWalkingLeft", true);
    }

    private void SetWalkingRight() {
        animator.SetBool("isWalking", true);
        animator.SetBool("isWalkingLeft", false);
        animator.SetBool("isWalkingRight", true);
    }

    private void SetIdle() {
        animator.SetTrigger("idle-trigger");
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalkingRight", false);
        animator.SetBool("isWalkingLeft", false);
    }

    private IEnumerator MeasureIdleTime() {
        while (true) {
            while (IsIdleAnimationPlaying()) {
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

    bool IsInState(string stateName) {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }

    private bool IsIdleAnimationPlaying() {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length > stateInfo.normalizedTime;
    }
}
