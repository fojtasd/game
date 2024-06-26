using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {

    // ====== ITEM DATA ======= //
    public string itemName;
    public string itemId;
    public int quantity;
    public Sprite sprite;
    public AudioClip pickupSound;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField] int maxNumberOfItems;


    // ====== ITEM SLOT ======= //
    [SerializeField] TMP_Text quantityText;
    [SerializeField] Image itemImage;

    public GameObject selectedShader;
    public bool isThisItemSelected;


    // ====== ITEM SLOT ======= //
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;


    // ====== INVENTORY MANAGER ======= //
    private InventoryManager inventoryManager;


    void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }
    }

    public int AddItem(string itemId, string itemName, int quantity, Sprite sprite, AudioClip pickupSound, string itemDescription) {
        if (isFull) {
            return quantity;
        }

        // update name
        this.itemName = itemName;
        this.itemId = itemId;

        // update image
        this.sprite = sprite;
        itemImage.sprite = sprite;

        // update description
        this.itemDescription = itemDescription;

        //update quantity
        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems) {
            this.pickupSound = pickupSound;
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        this.pickupSound = pickupSound;
        return 0;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnLeftClick();
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            OnRightClick();
        }
    }

    public void OnLeftClick() {
        if (isThisItemSelected) {
            inventoryManager.UseItem(itemId);
        }
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        isThisItemSelected = true;
        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = sprite;
        if (itemDescriptionImage.sprite == null) {
            itemDescriptionImage.sprite = emptySprite;
        }
    }


    public void OnRightClick() {
    }

}
