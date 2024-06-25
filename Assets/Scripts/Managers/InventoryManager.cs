using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject InventoryMenu;
    private bool menuActivated;

    public void ToggleInventory(InputAction.CallbackContext context) {
        menuActivated = !menuActivated;
        if (menuActivated) {
            //Time.timeScale = 0;
            InventoryMenu.SetActive(true);
        } else {
            //Time.timeScale = 1;
            InventoryMenu.SetActive(false);
        }
    }

    public void AddItem(string itemName, int quantity, Sprite sprite) {
        Debug.Log("Item added: " + itemName);
        Debug.Log("Quantity: " + quantity);
        Debug.Log("Sprite: " + sprite);
    }


}
