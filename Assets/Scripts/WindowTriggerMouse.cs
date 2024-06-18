using System.Collections;
using UnityEngine;

public class WindowTriggerMouse : MonoBehaviour {
    public Animator windowAnimator;
    public CustomCursor customCursor;
    private bool isPlayingAnimation = false;
    private bool isInside = false;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        if (customCursor == null) {
            customCursor = FindFirstObjectByType<CustomCursor>();
        }
    }

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

        if (!windowAnimator.GetBool("isOpening")) {
            windowAnimator.SetBool("isOpening", true);
            windowAnimator.SetBool("isClosing", false);
            PlaySound();
        }
        else {
            windowAnimator.SetBool("isOpening", false);
            windowAnimator.SetBool("isClosing", true);
            PlaySound();
        }

        // Počkejte, než animace doběhne
        while (windowAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
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
