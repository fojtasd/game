using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HudController2 : MonoBehaviour {
    private Animator headAnimator;

    public int currentHP = 100;

    public RuntimeAnimatorController faceAnimator100_81;
    public RuntimeAnimatorController faceAnimator80_61;
    public RuntimeAnimatorController faceAnimator60_41;
    public RuntimeAnimatorController faceAnimator40_21;
    public RuntimeAnimatorController faceAnimator21_1;

    private bool isLookingAnimationRunning = false;
    private bool isBlinkCoroutineRunning = false;
    private bool isBlinkingCoroutineRunning = false;
    private bool isAngerCouroutineRunning = false;
    private bool isSurpriseCouroutineRunning = false;
    private bool isFearCouroutineRunning = false;

    void Start() {
        headAnimator = GameObject.Find("HeadAnimator").GetComponent<Animator>();
        LoadAnimatorControllers();
        UpdateAnimatorController();
        StartRandomBlinking(5f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            currentHP += 10;
            UpdateAnimatorController();
            Debug.Log("Inreasing HP by 10" + ", current animator: " + headAnimator.runtimeAnimatorController.name + ", current HP: " + currentHP);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentHP -= 10;
            UpdateAnimatorController();
            Debug.Log("Decreasing HP by 10" + ", current animator: " + headAnimator.runtimeAnimatorController.name + ", current HP: " + currentHP);
        }
    }

    void OnEnable() {
        PlayerMovementInputSystem.Angry += StartTriggerAnger;
        HandlePlayerAnimations.OnLookDown += StartLookDown;
        HandlePlayerAnimations.OnLookUp += StartLookUp;
        HandlePlayerAnimations.OnLookLeft += StartLookLeft;
        HandlePlayerAnimations.OnLookRight += StartLookRight;
    }

    void OnDisable() {
        PlayerMovementInputSystem.Angry -= StartTriggerAnger;
        HandlePlayerAnimations.OnLookDown -= StartLookDown;
        HandlePlayerAnimations.OnLookUp -= StartLookUp;
        HandlePlayerAnimations.OnLookLeft -= StartLookLeft;
        HandlePlayerAnimations.OnLookRight -= StartLookRight;
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
            headAnimator.SetTrigger("Blink");
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            isBlinkCoroutineRunning = false;
        }
    }

    private IEnumerator LookLeft(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingLeft", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingLeft", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator LookRight(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingRight", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingRight", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator LookUp(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingUp", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingUp", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator LookDown(float delay) {
        if (!isLookingAnimationRunning) {
            isLookingAnimationRunning = true;
            headAnimator.SetInteger("priority", 2);
            headAnimator.SetBool("lookingDown", true);
            yield return new WaitForSeconds(delay);
            headAnimator.SetInteger("priority", 1);
            headAnimator.SetBool("lookingDown", false);
            float randomCoolDownDelay = Random.Range(0.5f, 2);
            yield return new WaitForSeconds(randomCoolDownDelay); // cannot be called for a certain duration
            isLookingAnimationRunning = false;
        }
    }

    private IEnumerator TriggerFear(float delay) {
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

    private IEnumerator TriggerAnger(float delay) {
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

    private IEnumerator TriggerSurprise(float delay) {
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

    private IEnumerator RandomlyCallBlink(float delay) {
        while (!isBlinkingCoroutineRunning) {
            isBlinkingCoroutineRunning = true;
            float randomDelay = Random.Range(2f, delay);
            yield return new WaitForSeconds(randomDelay);
            headAnimator.SetTrigger("Blink");
            isBlinkingCoroutineRunning = false;
        }
    }

    void UpdateAnimatorController() {
        switch (currentHP) {
            case int hp when hp > 80:
                headAnimator.runtimeAnimatorController = faceAnimator100_81;
                break;
            case int hp when hp > 60:
                headAnimator.runtimeAnimatorController = faceAnimator80_61;
                break;
            case int hp when hp > 40:
                headAnimator.runtimeAnimatorController = faceAnimator60_41;
                break;
            case int hp when hp > 20:
                headAnimator.runtimeAnimatorController = faceAnimator40_21;
                break;
            default:
                headAnimator.runtimeAnimatorController = faceAnimator21_1;
                break;
        }
    }

    void LoadAnimatorControllers() {
        faceAnimator100_81 = LoadAnimatorController("Assets/Animations/Player/Face/Player-head-controller100_81.controller");
        faceAnimator80_61 = LoadAnimatorController("Assets/Animations/Player/Face/Player-head-controller80_61.controller");
        faceAnimator60_41 = LoadAnimatorController("Assets/Animations/Player/Face/Player-head-controller60_41.controller");
        faceAnimator40_21 = LoadAnimatorController("Assets/Animations/Player/Face/Player-head-controller40_21.controller");
        faceAnimator21_1 = LoadAnimatorController("Assets/Animations/Player/Face/Player-head-controller20_1.controller");
    }

    RuntimeAnimatorController LoadAnimatorController(string path) {
        return AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(path);
    }
}
