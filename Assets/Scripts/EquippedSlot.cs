using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler {
    [SerializeField] Image slotImage;
    [SerializeField] TMP_Text slotName;
    [SerializeField] ItemType itemType;
    [SerializeField] Image playerDisplayImage;
    [SerializeField] public GameObject selectedShader;
    [SerializeField] public bool isThisItemSelected;

    private ItemSO itemSO;
    private bool slotIsInUse;

    private InventoryManager inventoryManager;

    private void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }
        slotIsInUse = false;
    }

    public void EquipGear(ItemSO itemSO) {
        if (slotIsInUse) {
            UnEquipGear();
        }
        this.itemSO = itemSO;
        slotImage.sprite = itemSO.itemPicture;
        slotName.enabled = false;

        if (itemSO.equippedItemSprite != null) {
            playerDisplayImage.sprite = itemSO.equippedItemSprite;
        } else {
            playerDisplayImage.sprite = itemSO.itemPicture;
        }


        slotIsInUse = true;
    }

    public void UnEquipGear() {
        if (itemSO == null) {
            return;
        }
        inventoryManager.DeselectAllEquippedSlots();
        inventoryManager.AddItemToInventory(1, itemSO);
        slotImage.sprite = itemSO.emptySprite;
        slotName.enabled = true;
        playerDisplayImage.sprite = itemSO.emptySprite;
        itemSO = null;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right) {
            OnRightClick();
        }
    }

    private void OnLeftClick() {
        if (isThisItemSelected) {
            UnEquipGear();
        } else {
            inventoryManager.DeselectAllEquippedSlots();
            selectedShader.SetActive(true);
            isThisItemSelected = true;
        }
    }

    private void OnRightClick() {
        UnEquipGear();
    }
}
