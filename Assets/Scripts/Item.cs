using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField] string itemName;
    [SerializeField] int quantity;
    [SerializeField] Sprite sprite;
    [SerializeField] InventoryManager inventoryManager;

    //private InventoryManager inventoryManager;
    // Start is called before the first frame update
    void Awake() {
        //inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }


    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(gameObject.name);
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Item picked up: " + itemName);
            inventoryManager.AddItem(itemName, quantity, sprite);
            Destroy(gameObject);
        }
    }
}
