using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrigger : MonoBehaviour {
    public Animator fanAnimator;
    private bool isInside = false;
    public CustomCursor customCursor;
    private AudioSource audioSource;
    public SimpleDialogBox dialogBox;

    private bool isPlayingAnimation = false;

    // Start is called before the first frame update
    void Start() {

        audioSource = GetComponent<AudioSource>();
        fanAnimator.SetBool("isOn", false);
        if (customCursor == null) {
            customCursor = FindFirstObjectByType<CustomCursor>();
        }
    }

    // Update is called once per frame
    void Update() {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero);

        if (hit.collider != null && Input.GetMouseButtonDown(0)) {
            if (hit.collider.gameObject == gameObject && !isPlayingAnimation) {
                StartCoroutine(PlayAnimation());
            }
        }

        // Check if the cursor is inside the trigger area
        if (hit.collider != null && hit.collider.gameObject == gameObject) {
            if (!isInside) {
                isInside = true;
                customCursor.SetHoverCursor();
            }
        }
        else {
            if (isInside) {
                isInside = false;
                customCursor.SetNormalCursor();
            }
        }
    }

    private IEnumerator PlayAnimation() {
        isPlayingAnimation = true;

        if (!fanAnimator.GetBool("isOn")) {
            fanAnimator.SetBool("isOn", true);
            fanAnimator.SetBool("isOff", false);
            dialogBox.SayLine("Why is that so loud?", voiceClipName: "a");
            PlaySound();
        }
        else {
            fanAnimator.SetBool("isOn", false);
            fanAnimator.SetBool("isOff", true);
            PlaySound();
        }

        // Počkejte, než animace doběhne
        while (fanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return null;
        }

        isPlayingAnimation = false;
    }

    public void PlaySound() {
        if (audioSource != null) {
            audioSource.Play();
        }
    }
}
