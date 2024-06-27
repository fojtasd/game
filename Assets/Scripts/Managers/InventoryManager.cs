using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour {
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlots;
    public ItemSO[] itemSOs;

    void Start() {
        InventoryMenu.SetActive(false);
    }

    public void ToggleInventory(InputAction.CallbackContext context) {
        menuActivated = !menuActivated;
        if (menuActivated) {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
        } else {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
        }
    }

    public int AddItemToInventory(int quantity, ItemSO itemSO) {
        for (int i = 0; i < itemSlots.Length; i++) {
            // zaloha piča !itemSlots[i].isFull && itemSlots[i].getItemSO().itemType == itemSO.itemType || itemSlots[i].quantity == 0
            if (itemSlots[i].GetItemSO() == null || !itemSlots[i].isFull && itemSlots[i].GetItemSO().itemType == itemSO.itemType || itemSlots[i].quantity == 0) {
                int leftOverItems = itemSlots[i].AddItemToSlot(quantity, itemSO);
                if (leftOverItems > 0) {
                    leftOverItems = AddItemToInventory(leftOverItems, itemSO);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public bool UseItem(ItemType itemType) {
        for (int i = 0; i < itemSOs.Length; i++) {
            if (itemSOs[i].itemType == itemType) {
                bool usable = itemSOs[i].UseItem();
                return usable;
            }
        }
        return false;
    }

    public void DeselectAllSlots() {
        for (int i = 0; i < itemSlots.Length; i++) {
            itemSlots[i].selectedShader.SetActive(false);
            itemSlots[i].isThisItemSelected = false;
        }
    }


}
