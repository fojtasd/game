using System.Collections;
using System.Collections.Generic;
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

    public int AddItem(string itemId, string itemName, int quantity, Sprite sprite, AudioClip pickupSound, string itemDescription) {
        for (int i = 0; i < itemSlots.Length; i++) {
            if (!itemSlots[i].isFull && itemSlots[i].name == name || itemSlots[i].quantity == 0) {
                int leftOverItems = itemSlots[i].AddItem(itemId, itemName, quantity, sprite, pickupSound, itemDescription);
                if (leftOverItems > 0) {
                    leftOverItems = AddItem(itemId, itemName, leftOverItems, sprite, pickupSound, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void UseItem(string itemId) {
        for (int i = 0; i < itemSOs.Length; i++) {
            if (itemSOs[i].itemId == itemId) {
                itemSOs[i].UseItem();
            }
        }

    }

    public void DeselectAllSlots() {
        for (int i = 0; i < itemSlots.Length; i++) {
            itemSlots[i].selectedShader.SetActive(false);
            itemSlots[i].isThisItemSelected = false;
        }
    }


}
