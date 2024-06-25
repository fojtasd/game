using UnityEngine;

public class CustomCursor : MonoBehaviour {
    CursorManager cursorManager;

    public void Setup(CursorManager cursorManager) {
        this.cursorManager = cursorManager;
    }

    void Update() {
        cursorManager.HandleCursorLogic();
    }
}
