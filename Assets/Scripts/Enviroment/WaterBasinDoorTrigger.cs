using System.Collections;
using UnityEngine;

public class WaterBasinTrigger : MonoBehaviour {
    [SerializeField] string messageToSay = "What the fuck is this filth?";
    [SerializeField] AudioClipName audioClipName;
    public delegate void WaterBasinAction(bool isOn);
    public static event WaterBasinAction OnWaterBasinOpenedLight;

    AudioSource audioSource;
    Animator waterBasinAnimator;
    bool isCoroutinePlaying = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        waterBasinAnimator = transform.parent.GetComponent<Animator>();
        waterBasinAnimator.SetBool("isClosing", false);
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

        if (!waterBasinAnimator.GetBool("isOpening")) {
            waterBasinAnimator.SetBool("isOpening", true);
            waterBasinAnimator.SetBool("isClosing", false);
            PlaySound();
            Utils.Say(messageToSay, voiceClipName: audioClipName);
            OnWaterBasinOpenedLight?.Invoke(true);
        } else {
            waterBasinAnimator.SetBool("isOpening", false);
            waterBasinAnimator.SetBool("isClosing", true);
            PlaySound();
            OnWaterBasinOpenedLight?.Invoke(false);
        }

        while (waterBasinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        isCoroutinePlaying = false;
    }

    public void PlaySound() {
        if (audioSource != null) {
            audioSource.Play();
        }
    }
}
