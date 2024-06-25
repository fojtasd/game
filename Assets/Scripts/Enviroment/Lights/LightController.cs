using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour {
    public Light2D bulbLight;
    public Animator bulbAnimator;

    [SerializeField] bool isBlinking = false;
    [SerializeField] float minBlinkDelay = 1.0f;
    [SerializeField] float maxBlinkDelay = 5.0f;
    [SerializeField] bool isOn = false;

    void Start() {
        bulbLight.enabled = isOn;
        if (isOn) {
            bulbAnimator.SetBool("LightingOn", true);
            bulbAnimator.SetBool("LightingOff", false);
        }
        else {
            bulbAnimator.SetBool("LightingOff", true);
            bulbAnimator.SetBool("LightingOn", false);
        }
        if (isBlinking) {
            bulbAnimator.SetBool("isBlinking", true);
        }

        if (isBlinking) {
            bulbAnimator.SetBool("isBlinking", true);
            StartCoroutine(BlinkRoutine());
        }
    }

    void TurnOnLight() {
        bulbAnimator.SetBool("LightingOn", true);
        bulbAnimator.SetBool("LightingOff", false);
        if (isBlinking) {
            StartCoroutine(BlinkRoutine());
        }
        else {
            StopCoroutine(BlinkRoutine());
        }
        bulbLight.enabled = true;
    }

    void TurnOffLight() {
        bulbAnimator.SetBool("LightingOn", false);
        bulbAnimator.SetBool("LightingOff", true);
        if (isBlinking) {
            StopCoroutine(BlinkRoutine());
        }
        bulbLight.enabled = false;
    }

    void AnimatorLightOn() {
        bulbLight.enabled = true;
    }

    void AnimatorLightOff() {
        bulbLight.enabled = false;
    }

    IEnumerator BlinkRoutine() {
        while (true) {
            float delay = Random.Range(minBlinkDelay, maxBlinkDelay);
            yield return new WaitForSeconds(delay);

            if (isBlinking) {
                int randomBlink = Random.Range(0, 2);
                if (randomBlink == 0) {
                    bulbAnimator.SetTrigger("HeavyBlink");
                }
                else {
                    bulbAnimator.SetTrigger("DecentBlink");
                }
            }
        }
    }


}
