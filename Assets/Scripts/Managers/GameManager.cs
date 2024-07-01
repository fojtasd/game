using UnityEngine;

public class GameManager : MonoBehaviour {
    public HealthBar healthBar;
    public FaceController faceController;
    public PortraitController portraitController;
    public CustomCursor customCursor;
    public AmmoBar ammoBar;
    public SOManager itemManagerSO;
    public PlayerStatsManager playerStatsManager;
    public InventoryManager inventoryManager;

    void Awake() {
        HealthManager healthManager = new(100, 100);
        AmmoManager ammoManager = new(0, 100);
        CursorManager cursorManager = new();

        healthBar.Setup(healthManager);
        faceController.Setup(healthManager);
        portraitController.Setup(healthManager);
        playerStatsManager.Setup(healthManager);

        customCursor.Setup(cursorManager);

        ammoBar.Setup(ammoManager);

        itemManagerSO.Setup(healthManager, ammoManager);
        Setup();
    }

    void Setup() {
        Item.OnPlayerTouched += delegate (Item item) {
            int initialQuantity = item.quantity;
            inventoryManager.AddItemToInventory(item);
            bool wasItemCollected = item.quantity != initialQuantity;
            if (wasItemCollected) {
                SoundManager.Instance.PlaySound(item.itemSO.pickupSound);
            }
            if (item.quantity == 0) {
                Destroy(item.gameObject);
            }
        };
    }
}
