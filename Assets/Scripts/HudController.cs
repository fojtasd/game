using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HudController : MonoBehaviour {

    private GameObject hudManager;
    private Animator hudAnimator;

    private bool isAnimationRunning = false;
    private bool isLookingAnimationRunning = false;

    private bool isBlinkCoroutineRunning = false;
    private bool isBlinkingCoroutineRunning = false;
    private bool isAngerCouroutineRunning = false;
    private bool isSurpriseCouroutineRunning = false;
    private bool isFearCouroutineRunning = false;

    void Start() {
        hudAnimator.SetInteger("priority", 1);
    }

    void Awake() {
        hudManager = GameObject.Find("Head-animator");
        hudAnimator = hudManager.GetComponent<Animator>();
    }

    public void StartBlink(float delay = 0f) {
        StartCoroutine(Blink());
    }

    public void StartRandomBlinking(float delay = 0f) {
        if (!isBlinkingCoroutineRunning) {
            StartCoroutine(RandomlyCallBlink(delay));
        }
    }

    public void StartLookLeft(float delay = 0.5f) {
        StartCoroutine(LookLeft(delay));
    }

    public void StartLookRight(float delay = 0.5f) {
        StartCoroutine(LookRight(delay));
    }


    public void StartLookUp(float delay = 0.5f) {
        StartCoroutine(LookUp(delay));
    }

    public void StartLookDown(float delay = 0.5f) {
        StartCoroutine(LookDown(delay));
    }

    public void StartTriggerFear(float delay = 0.5f) {
        StartCoroutine(TriggerFear(delay));
    }

    public void StartTriggerAnger(float delay = 0.5f) {
        StartCoroutine(TriggerAnger(delay));
    }

    public void StartTriggerSurprise(float delay = 0.5f) {
        StartCoroutine(TriggerSurprise(delay));
    }

    private IEnumerator Blink(float delay = 0) {
        if (!isBlinkCoroutineRunning) {
            isBlinkCoroutineRunning = true;
            hudAnimator.SetTrigger("Blink");
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            isBlinkCoroutineRunning = false;
        }
    }

    private IEnumerator LookLeft(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            hudAnimator.SetInteger("priority", 2);
            hudAnimator.SetBool("lookingLeft", true);
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            hudAnimator.SetBool("lookingLeft", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator LookRight(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            hudAnimator.SetInteger("priority", 2);
            hudAnimator.SetBool("lookingRight", true);
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            hudAnimator.SetBool("lookingRight", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator LookUp(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            hudAnimator.SetInteger("priority", 2);
            hudAnimator.SetBool("lookingUp", true);
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            hudAnimator.SetBool("lookingUp", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator LookDown(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            hudAnimator.SetInteger("priority", 2);
            hudAnimator.SetBool("lookingDown", true);
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            hudAnimator.SetBool("lookingDown", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator TriggerFear(float delay) {
        if (!isFearCouroutineRunning) {
            isFearCouroutineRunning = true;
            hudAnimator.SetBool("isTerrified", true);
            hudAnimator.SetInteger("priority", 3);
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            hudAnimator.SetBool("isTerrified", false);
            isFearCouroutineRunning = false;
        }
    }

    private IEnumerator TriggerAnger(float delay) {
        if (!isAngerCouroutineRunning) {
            isAngerCouroutineRunning = true;
            hudAnimator.SetBool("isAngry", true);
            hudAnimator.SetInteger("priority", 3);
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            hudAnimator.SetBool("isAngry", false);
            isAngerCouroutineRunning = false;
        }
    }

    private IEnumerator TriggerSurprise(float delay) {
        if (!isSurpriseCouroutineRunning) {
            isSurpriseCouroutineRunning = true;
            hudAnimator.SetBool("isSurprised", true);
            hudAnimator.SetInteger("priority", 3);
            yield return new WaitForSeconds(delay);
            hudAnimator.SetInteger("priority", 1);
            hudAnimator.SetBool("isSurprised", false);
            isSurpriseCouroutineRunning = false;
        }
    }

    private IEnumerator RandomlyCallBlink(float delay) {
        while (!isBlinkingCoroutineRunning) {
            isBlinkingCoroutineRunning = true;
            float randomDelay = Random.Range(2f, delay);
            yield return new WaitForSeconds(randomDelay);
            hudAnimator.SetTrigger("Blink");
            isBlinkingCoroutineRunning = false;
        }
    }
}
