using UnityEngine;

public class GameManager : MonoBehaviour {
    public HealthBar healthBar;
    public FaceController faceController;
    public CustomCursor customCursor;
    public AmmoBar ammoBar;
    public SOManager itemManagerSO;

    void Start() {
        HealthManager healthManager = new(100, 100);
        AmmoManager ammoManager = new(0, 100);
        CursorManager cursorManager = new();

        healthBar.Setup(healthManager);
        faceController.Setup(healthManager);

        customCursor.Setup(cursorManager);

        ammoBar.Setup(ammoManager);

        itemManagerSO.Setup(healthManager, ammoManager);
    }
}
