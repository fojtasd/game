using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour {
    public GameObject EquipmentMenu;
    public InventorySlot[] inventorySlots;
    public EquipmentSlot[] equippedSlots;
    public ItemSO[] itemSOs;

    void Start() {
        EquipmentMenu.SetActive(false);
        InventorySlot.OnItemDropped += ReorganizeInventory;
    }

    private void ReorganizeInventory() {
        int leftIndex = 0;
        int rightIndex = inventorySlots.Length - 1;

        while (leftIndex < rightIndex) {
            bool isLeftFree = inventorySlots[leftIndex].IsSlotFree();
            bool isRightFree = inventorySlots[rightIndex].IsSlotFree();

            if (isLeftFree && !isRightFree) {
                inventorySlots[leftIndex].SetStoredItem(inventorySlots[rightIndex].GetStoredItem().Clone());
                inventorySlots[rightIndex].EmptySlot();
                inventorySlots[leftIndex].UpdateVisuals();
                leftIndex++;
                rightIndex--;
                continue;
            }

            if (!isLeftFree) {
                leftIndex++;
            }
            if (isRightFree) {
                rightIndex--;
            }
        }
    }

    public void ToggleInventory(InputAction.CallbackContext context) {
        if (EquipmentMenu.activeSelf) {
            Time.timeScale = 1;
            EquipmentMenu.SetActive(false);
        } else {
            Time.timeScale = 0;
            EquipmentMenu.SetActive(true);
        }
    }

    public void AddItemToInventory(Item item) {
        foreach (InventorySlot slot in inventorySlots) {
            item = slot.AddItemToSlot(item);
            if (item == null) {
                return;
            }
        }
    }

    public bool UseItem(ItemId itemId) {
        for (int i = 0; i < itemSOs.Length; i++) {
            if (itemSOs[i].itemId == itemId) {
                bool usable = itemSOs[i].UseItem();
                return usable;
            }
        }
        return false;
    }

    public void DeselectInventorySlots() {
        for (int i = 0; i < inventorySlots.Length; i++) {
            inventorySlots[i].selectedShader.SetActive(false);
            inventorySlots[i].isThisSlotSelected = false;
        }
    }

    public void DeselectAllEquipmentSlots() {
        for (int i = 0; i < equippedSlots.Length; i++) {
            equippedSlots[i].selectedShader.SetActive(false);
            equippedSlots[i].isThisSlotSelected = false;
        }
    }
}

public enum ItemType {
    none,
    ammo,
    consumable,
    crafting,
    collectible,
    head,
    shirt,
    body,
    legs,
    mainHand,
    offHand,
    relic,
    feet
};