using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField] string itemName;
    [SerializeField] string itemId;

    [SerializeField][TextArea] string itemDescription;
    [SerializeField] int quantity;
    [SerializeField] Sprite sprite;
    [SerializeField] AudioClip pickupSound;
    InventoryManager inventoryManager;

    void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }
        GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            int leftOverItems = inventoryManager.AddItem(itemId, itemName, quantity, sprite, pickupSound, itemDescription);
            if (leftOverItems <= 0) {
                SoundManager.Instance.PlaySound(pickupSound);
                Destroy(gameObject);
            } else {
                quantity = leftOverItems;
            }


        }
    }
}
