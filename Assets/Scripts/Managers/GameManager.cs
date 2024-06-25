using UnityEngine;

public class GameManager : MonoBehaviour {
    public HealthBar healthBar;
    public FaceController faceController;
    public CustomCursor customCursor;

    void Start() {
        HealthManager healthManager = new(100, 100);
        healthBar.Setup(healthManager);
        faceController.Setup(healthManager);

        CursorManager cursorManager = new();
        customCursor.Setup(cursorManager);
    }
}
