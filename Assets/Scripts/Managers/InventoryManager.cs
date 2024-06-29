using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour {
    public GameObject EquipmentMenu;
    public EquipmentSlot[] equipmentSlots;
    public EquippedSlot[] equippedSlots;
    public ItemSO[] itemSOs;

    void Start() {
        EquipmentMenu.SetActive(false);
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

    public int AddItemToInventory(int quantity, ItemSO itemSO) {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            if (equipmentSlots[i].GetItemSO() == null || !equipmentSlots[i].isFull && equipmentSlots[i].GetItemSO().itemId == itemSO.itemId || equipmentSlots[i].quantity == 0) {
                int leftOverItems = equipmentSlots[i].AddItemToSlot(quantity, itemSO);
                if (leftOverItems > 0) {
                    leftOverItems = AddItemToInventory(leftOverItems, itemSO);
                }
                return leftOverItems;
            }
        }
        return quantity;
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

    public void DeselectAllEquipmentSlots() {
        for (int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlots[i].selectedShader.SetActive(false);
            equipmentSlots[i].isThisItemSelected = false;
        }
    }

    public void DeselectAllEquippedSlots() {
        for (int i = 0; i < equippedSlots.Length; i++) {
            equippedSlots[i].selectedShader.SetActive(false);
            equippedSlots[i].isThisItemSelected = false;
        }
    }

    private bool IsEquipment(ItemType itemType) {
        return itemType switch {
            ItemType.ammo => false,
            ItemType.consumable => false,
            ItemType.crafting => false,
            ItemType.collectible => false,
            ItemType.head => true,
            ItemType.shirt => true,
            ItemType.body => true,
            ItemType.legs => true,
            ItemType.mainHand => true,
            ItemType.offHand => true,
            ItemType.relic => true,
            ItemType.feet => true,
            _ => false,
        };
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