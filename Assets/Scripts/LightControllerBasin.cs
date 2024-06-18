using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControllerBasin : MonoBehaviour {
    public Light2D bulbLight;
    public Animator bulbAnimator;

    public bool isBlinking = false;
    // Default minimum is 1.0f and maximum is 5.0f
    public float minBlinkDelay = 1.0f;  // Minimum delay between blinks
    public float maxBlinkDelay = 5.0f;  // Maximum delay between blinks
    public bool isOn = false;

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
            StartCoroutine(BlinkRoutine());
        }

    }

    void OnEnable() {
        WaterBasinTrigger.OnWaterBasinOpened += TurnOnLight;
        WaterBasinTrigger.OnWaterBasinClosed += TurnOffLight;
    }

    void OnDisable() {
        WaterBasinTrigger.OnWaterBasinOpened -= TurnOnLight;
        WaterBasinTrigger.OnWaterBasinClosed -= TurnOffLight;
    }

    private IEnumerator BlinkRoutine() {
        while (true) {
            // Wait for a random amount of time between blinks
            float delay = Random.Range(minBlinkDelay, maxBlinkDelay);
            yield return new WaitForSeconds(delay);

            if (isBlinking) {
                // Randomly select which blink animation to play
                int randomBlink = Random.Range(0, 2); // 0 or 1

                if (randomBlink == 0) {
                    bulbAnimator.SetTrigger("HeavyBlink");
                }
                else {
                    bulbAnimator.SetTrigger("DecentBlink");
                }
            }
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


}
