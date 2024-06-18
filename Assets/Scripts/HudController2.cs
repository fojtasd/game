using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HudController2 : MonoBehaviour {
    private Animator headAnimator;

    private bool isAngerCouroutineRunning = false;
    void Start() {

    }

    void Awake() {
        headAnimator = GameObject.Find("Head-animator").GetComponent<Animator>();
    }

    void OnEnable() {
        PlayerMovementInputSystem.Angry += StartTriggerAnger;
    }

    void OnDisable() {
        PlayerMovementInputSystem.Angry -= StartTriggerAnger;
    }

    public void StartTriggerAnger(float delay = 0.5f) {
        StartCoroutine(TriggerAnger(delay));
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
}
