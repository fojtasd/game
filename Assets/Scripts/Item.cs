using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour {
    public ItemSO itemSO;
    public int quantity;

    InventoryManager inventoryManager;

    void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }

    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            int leftOverItems = inventoryManager.AddItemToInventory(quantity, itemSO);
            if (leftOverItems <= 0) {
                SoundManager.Instance.PlaySound(itemSO.pickupSound);
                Destroy(gameObject);
            } else {
                quantity = leftOverItems;
            }
        }
    }
}
