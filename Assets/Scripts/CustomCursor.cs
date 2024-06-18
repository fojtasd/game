using UnityEngine;

public class CustomCursor : MonoBehaviour {
    public Texture2D cursorTextureNormal;
    public Animator cursorAnimator;
    public RuntimeAnimatorController hoverAnimationController;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start() {
        SetNormalCursor();
    }

    public void SetNormalCursor() {
        Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode);
        Cursor.visible = true;
        cursorAnimator.gameObject.SetActive(false); // Hide the animated cursor
    }

    public void SetHoverCursor() {
        Cursor.SetCursor(null, Vector2.zero, cursorMode); // Hide default cursor
        Cursor.visible = false; // Hide the default white cursor
        cursorAnimator.gameObject.SetActive(true);
        cursorAnimator.runtimeAnimatorController = hoverAnimationController; // Play animation
        UpdateCursorPosition();
    }

    void Update() {
        Cursor.visible = false;
        if (cursorAnimator.gameObject.activeSelf) {
            UpdateCursorPosition();
        }
    }

    private void UpdateCursorPosition() {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorAnimator.transform.position = cursorPos;
    }
}
