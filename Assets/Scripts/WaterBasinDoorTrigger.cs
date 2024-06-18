using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBasinTrigger : MonoBehaviour {
    public Animator waterBasinAnimator;

    public HudController hudController;

    public SimpleDialogBox dialogBox;
    public delegate void WaterBasinAction();
    public static event WaterBasinAction OnWaterBasinOpened;
    public static event WaterBasinAction OnWaterBasinClosed;

    private bool isInside = false;
    public CustomCursor customCursor;
    private AudioSource audioSource;
    public PlayerSoundsLibrary playerSoundsLibrary;

    private bool isPlayingAnimation = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        waterBasinAnimator.SetBool("isClosing", false);
        if (customCursor == null) {
            customCursor = FindFirstObjectByType<CustomCursor>();
        }
    }

    // Update is called once per frame
    void Update() {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero);

        if (hit.collider != null && Input.GetMouseButtonDown(0) && !isPlayingAnimation) {
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

        if (!waterBasinAnimator.GetBool("isOpening")) {
            waterBasinAnimator.SetBool("isOpening", true);
            waterBasinAnimator.SetBool("isClosing", false);
            PlaySound();
            dialogBox.SayLine("What the fuck is this filth?", voiceClipName: "a");
            OnWaterBasinOpened?.Invoke();
        }
        else {
            waterBasinAnimator.SetBool("isOpening", false);
            waterBasinAnimator.SetBool("isClosing", true);
            PlaySound();
            OnWaterBasinClosed?.Invoke();
        }

        while (waterBasinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        isPlayingAnimation = false;
    }

    public void PlaySound() {
        if (audioSource != null) {
            audioSource.Play();
        }
    }
}
