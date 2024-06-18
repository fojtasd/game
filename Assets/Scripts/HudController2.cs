using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HudController2 : MonoBehaviour {
    private Animator hudAnimator;

    private bool isAngerCouroutineRunning = false;
    void Start() {

    }

    void Awake() {
        hudAnimator = GameObject.Find("Head-animator").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        PlayerMovementInputSystem.Angry += () => TriggerAnger(1f);
    }

    void OnDisable() {
        PlayerMovementInputSystem.Angry -= () => TriggerAnger(1f);
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
}
