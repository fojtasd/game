using System.Collections;
using UnityEngine;

public class FanTrigger : MonoBehaviour {
    [SerializeField] string messageToSay = "Why is that so loud?";
    [SerializeField] AudioClipName audioClipName;
    Animator fanAnimator;
    AudioSource audioSource;
    bool isCoroutinePlaying = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        fanAnimator = transform.parent.GetComponent<Animator>();
        fanAnimator.SetBool("isOn", false);
    }

    void OnEnable() {
        CursorManager.InteractingWithObjectEvent += Interaction;
    }

    void OnDisable() {
        CursorManager.InteractingWithObjectEvent -= Interaction;
    }

    void Interaction(GameObject interactedObject) {
        if (interactedObject == gameObject && !isCoroutinePlaying) {
            StartCoroutine(Interact());
        }
    }

    IEnumerator Interact() {
        isCoroutinePlaying = true;

        if (!fanAnimator.GetBool("isOn")) {
            fanAnimator.SetBool("isOn", true);
            fanAnimator.SetBool("isOff", false);
            Utils.Say(messageToSay, voiceClipName: audioClipName);
            PlaySound();
        }
        else {
            fanAnimator.SetBool("isOn", false);
            fanAnimator.SetBool("isOff", true);
            PlaySound();
        }

        while (fanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return null;
        }

        isCoroutinePlaying = false;
    }

    public void PlaySound() {
        if (audioSource != null) {
            audioSource.Play();
        }
    }
}
