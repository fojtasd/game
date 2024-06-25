using System.Collections;
using UnityEngine;

public class WindowTrigger : MonoBehaviour {
    [SerializeField] string messageToSay = "";
    [SerializeField] AudioClipName audioClipName;
    Animator windowAnimator;
    bool isCoroutinePlaying = false;
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        windowAnimator = transform.parent.GetComponent<Animator>();
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

        if (!windowAnimator.GetBool("isOpening")) {
            Utils.Say(messageToSay, voiceClipName: audioClipName);
            windowAnimator.SetBool("isOpening", true);
            windowAnimator.SetBool("isClosing", false);
            PlaySound();
        }
        else {
            windowAnimator.SetBool("isOpening", false);
            windowAnimator.SetBool("isClosing", true);
            PlaySound();
        }

        while (windowAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
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
