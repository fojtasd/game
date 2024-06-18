using UnityEngine;
using TMPro;
using System.Collections;

public class Pokus : MonoBehaviour {
    public GameObject textBubble;
    private TextMeshProUGUI bubbleText;
    private RectTransform textBubbleRect;
    private SpriteRenderer bubbleBackgroundRenderer;

    public float countdownTime = 10f; // Time to countdown in seconds
    private float elapsedTime;

    void Start() {
        // Find and assign the TextMeshPro component from the textBubble
        bubbleText = textBubble.GetComponentInChildren<TextMeshProUGUI>();

        // Find and assign the RectTransform component of the textBubble
        textBubbleRect = textBubble.GetComponent<RectTransform>();

        // Find the BubbleBackground's SpriteRenderer
        Transform bubbleBackgroundTransform = textBubble.transform.Find("BubbleBackground");
        if (bubbleBackgroundTransform != null) {
            bubbleBackgroundRenderer = bubbleBackgroundTransform.GetComponent<SpriteRenderer>();
        }

        // Initially hide the text bubble
        //textBubble.SetActive(false);
        StartCoroutine(IntervalTriggerCoroutine());
    }

    public void ShowTextBubble(string message) {
        textBubble.SetActive(true);
        if (bubbleText != null) {
            bubbleText.text = message;
            // Force the layout to update
            Canvas.ForceUpdateCanvases();

            // Update the size of BubbleBackground
            UpdateBubbleBackgroundSize();
        }
    }

    public void HideTextBubble() {
        if (bubbleText != null) {
            bubbleText.text = string.Empty;
            textBubble.SetActive(false);
        }
    }

    private void UpdateBubbleBackgroundSize() {
        if (bubbleBackgroundRenderer != null && textBubbleRect != null) {
            // Update the size of the sprite based on the size of the TextBubble RectTransform
            bubbleBackgroundRenderer.size = new Vector2(textBubbleRect.rect.width, textBubbleRect.rect.height);
        }
    }

    IEnumerator IntervalTriggerCoroutine() {
        while (true) {
            yield return new WaitForSeconds(3); // Wait for the interval
            ShowTextBubble("Triggered after 3 seconds");
            yield return new WaitForSeconds(3); // Wait for the interval
            HideTextBubble();
            yield return new WaitForSeconds(3f); // Show message for 2 seconds
            ShowTextBubble("Extra pičo");
            yield return new WaitForSeconds(3f); // Show message for 2 seconds
            HideTextBubble();
        }
    }
}
