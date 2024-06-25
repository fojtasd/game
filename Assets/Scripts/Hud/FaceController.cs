using System.Collections;
using UnityEngine;

public class FaceController : MonoBehaviour {
    Animator headAnimator;
    HealthManager healthManager;

    bool isLookingAnimationRunning = false;
    bool isBlinkCoroutineRunning = false;
    bool isBlinkingCoroutineRunning = false;
    bool isAngerCouroutineRunning = false;
    bool isSurpriseCouroutineRunning = false;
    bool isFearCouroutineRunning = false;

    Coroutine talkingCoroutine = null;

    void Awake() {
        if (!GameObject.FindWithTag("HeadController").TryGetComponent<Animator>(out headAnimator)) {
            Debug.LogError("Head Animator not found!");
        }
    }
    void Start() {
        StartRandomBlinking(5f);
    }

    void Update() {
        UpdateLayerWeights(healthManager.GetHealthState());
    }

    public void Setup(HealthManager healthManager) {
        this.healthManager = healthManager;
    }

    void OnEnable() {
        PlayerMovementInputSystem.Angry += StartTriggerAnger;
        HandlePlayerAnimations.OnLookDown += StartLookDown;
        HandlePlayerAnimations.OnLookUp += StartLookUp;
        HandlePlayerAnimations.OnLookLeft += StartLookLeft;
        HandlePlayerAnimations.OnLookRight += StartLookRight;
        MessageToSay.TalkingEvent += StartTriggerTalking;
    }

    void OnDisable() {
        PlayerMovementInputSystem.Angry -= StartTriggerAnger;
        HandlePlayerAnimations.OnLookDown -= StartLookDown;
        HandlePlayerAnimations.OnLookUp -= StartLookUp;
        HandlePlayerAnimations.OnLookLeft -= StartLookLeft;
        HandlePlayerAnimations.OnLookRight -= StartLookRight;
        MessageToSay.TalkingEvent -= StartTriggerTalking;
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

    public void StartTriggerTalking(bool isTalking) {
        talkingCoroutine = StartCoroutine(TriggerTalking(isTalking));
    }

    IEnumerator Blink(float delay = 0) {
        if (!isBlinkCoroutineRunning) {
            isBlinkCoroutineRunning = true;
            headAnimator.SetTrigger("Blink");
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            isBlinkCoroutineRunning = false;
        }
    }

    IEnumerator LookLeft(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingLeft", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingLeft", false);
            isLookingAnimationRunning = false;
        }
    }

    IEnumerator LookRight(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingRight", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingRight", false);
            isLookingAnimationRunning = false;
        }
    }

    IEnumerator LookUp(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingUp", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingUp", false);
            isLookingAnimationRunning = false;
        }
    }

    IEnumerator LookDown(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingDown", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingDown", false);
            isLookingAnimationRunning = false;
        }
    }

    IEnumerator TriggerFear(float delay) {
        if (!isFearCouroutineRunning) {
            isFearCouroutineRunning = true;
            headAnimator.SetBool("isTerrified", true);
            headAnimator.SetInteger("priority", 3);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("isTerrified", false);
            isFearCouroutineRunning = false;
        }
    }

    IEnumerator TriggerAnger(float delay) {
        if (!isAngerCouroutineRunning) {
            isAngerCouroutineRunning = true;
            headAnimator.SetBool("isAngry", true);
            headAnimator.SetInteger("priority", 3);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("isAngry", false);
            isAngerCouroutineRunning = false;
        }
    }

    IEnumerator TriggerSurprise(float delay) {
        if (!isSurpriseCouroutineRunning) {
            isSurpriseCouroutineRunning = true;
            headAnimator.SetBool("isSurprised", true);
            headAnimator.SetInteger("priority", 3);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("isSurprised", false);
            isSurpriseCouroutineRunning = false;
        }
    }

    IEnumerator RandomlyCallBlink(float delay) {
        while (!isBlinkingCoroutineRunning) {
            isBlinkingCoroutineRunning = true;
            float randomDelay = Random.Range(2f, delay);
            yield return new WaitForSeconds(randomDelay);
            headAnimator.SetTrigger("Blink");
            isBlinkingCoroutineRunning = false;
        }
    }

    IEnumerator TriggerTalking(bool isTalking) {
        if (!isTalking && talkingCoroutine != null) {
            headAnimator.SetBool("isTalking", false);
            headAnimator.SetInteger("priority", 1);
            StopCoroutine(talkingCoroutine);
            yield break;
        }
        headAnimator.SetInteger("priority", 4);
        headAnimator.SetBool("isTalking", true);
        while (isTalking) {
            int randomInt = Random.Range(0, 3);
            headAnimator.SetInteger("talkingAnimationId", randomInt);
            yield return new WaitForSeconds(0.5f);
        }
        headAnimator.SetBool("isTalking", false);
        headAnimator.SetInteger("priority", 1);
    }

    void UpdateLayerWeights(HealthManager.HealthState healthState) {
        switch (healthState) {
            case HealthManager.HealthState.Healthy:
                SetLayerWeights(1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case HealthManager.HealthState.SlighlyHurt:
                SetLayerWeights(0.0f, 1f, 0.0f, 0.0f, 0.0f);
                break;
            case HealthManager.HealthState.Hurt:
                SetLayerWeights(0.0f, 0.0f, 1f, 0.0f, 0.0f);
                break;
            case HealthManager.HealthState.BadlyHurt:
                SetLayerWeights(0.0f, 0.0f, 0.0f, 1f, 0.0f);
                break;
            case HealthManager.HealthState.Critical:
                SetLayerWeights(0.0f, 0.0f, 0.0f, 0.0f, 1f);
                break;
            case HealthManager.HealthState.Death:
                SetLayerWeights(0.0f, 0.0f, 0.0f, 0.0f, 1f);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    void SetLayerWeights(float layer0Weight, float layer1Weight, float layer2Weight, float layer3Weight, float layer4Weight) {
        headAnimator.SetLayerWeight(0, layer0Weight);
        headAnimator.SetLayerWeight(1, layer1Weight);
        headAnimator.SetLayerWeight(2, layer2Weight);
        headAnimator.SetLayerWeight(3, layer3Weight);
        headAnimator.SetLayerWeight(4, layer4Weight);
    }
}
