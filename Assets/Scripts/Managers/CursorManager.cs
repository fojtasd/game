using UnityEngine;

public class CursorManager {
    public Texture2D normalCursor;
    public Texture2D aimCursor;
    public Animator cursorAnimator;
    private readonly GameObject customCursor;
    public RuntimeAnimatorController cursorAnimationController;
    public SpriteRenderer cursorSpriteRenderer;
    public Vector2 hotSpot;
    public CursorMode cursorMode;
    public delegate void InteractingWithObjectDelegate(GameObject interactedObject);
    public static event InteractingWithObjectDelegate InteractingWithObjectEvent;


    CursorState cursorState;

    public CursorManager() {
        normalCursor = Resources.Load<Texture2D>("Sprites/Cursor/cursor-normal");
        cursorAnimationController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Cursor/CursorAnimationController");
        aimCursor = Resources.Load<Texture2D>("Sprites/Cursor/cursor-aim");
        hotSpot = Vector2.zero;
        cursorMode = CursorMode.Auto;
        cursorState = CursorState.Normal;
        customCursor = GameObject.FindWithTag("Cursor");
        cursorAnimator = customCursor.GetComponentInChildren<Animator>();
        cursorSpriteRenderer = customCursor.GetComponentInChildren<SpriteRenderer>();
        SetNormalCursor();

        PlayerMovementInputSystem.DrawWeaponEvent += SetCursorToAimState;
        PlayerMovementInputSystem.HolsterWeaponEvent += SetCursorToNormalState;
    }

    public void HandleCursorLogic() {
        UpdateCursorType();
        SetCursorByState();
        HandleCursorInteractions();
    }

    public bool IsCursorOverInteractables() {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(cursorPos, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Interactables")) {
            return true;
        }
        else {
            return false;
        }
    }

    public void HandleCursorInteractions() {
        if (cursorState == CursorState.Interaction && Input.GetMouseButtonDown(0)) {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(cursorPos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Interactables")) {
                InteractingWithObjectEvent?.Invoke(hit.collider.gameObject);
            }
        }
    }

    void SetCursorState(CursorState cursorState) {
        this.cursorState = cursorState;
    }

    private void SetCursorToAimState() {
        SetCursorState(CursorState.Aim);
    }

    void SetCursorToNormalState() {
        SetCursorState(CursorState.Normal);
    }

    void SetCursorByState() {
        switch (cursorState) {
            case CursorState.Normal:
                SetNormalCursor();
                break;
            case CursorState.Interaction:
                SetInteractionCursor();
                break;
            case CursorState.Aim:
                SetAimCursor();
                break;
        }
    }

    void UpdateCursorType() {
        if (cursorState == CursorState.Aim) {
            return;
        }
        switch (IsCursorOverInteractables()) {
            case true:
                SetCursorState(CursorState.Interaction);
                break;
            case false:
                SetCursorState(CursorState.Normal);
                break;
        }
    }

    void UpdateCursorPosition() {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorAnimator.transform.position = cursorPos;
    }

    void SetNormalCursor() {
        DisableAnimatorAndSpriteRenderer();
        Cursor.SetCursor(normalCursor, hotSpot, cursorMode);
        Cursor.visible = true;
    }

    void SetInteractionCursor() {
        EnableAnimatorAndSpriteRenderer();
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        Cursor.visible = false;
        cursorAnimator.runtimeAnimatorController = cursorAnimationController;
        UpdateCursorPosition();
    }

    void SetAimCursor() {
        DisableAnimatorAndSpriteRenderer();
        Vector2 aimHotSpot = new(aimCursor.width / 2, aimCursor.height / 2);
        Cursor.SetCursor(aimCursor, aimHotSpot, cursorMode);
        Cursor.visible = true;
        UpdateCursorPosition();
    }

    void DisableAnimatorAndSpriteRenderer() {
        cursorAnimator.enabled = false;
        cursorSpriteRenderer.enabled = false;
    }

    void EnableAnimatorAndSpriteRenderer() {
        cursorAnimator.enabled = true;
        cursorSpriteRenderer.enabled = true;
    }
}
