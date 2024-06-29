using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Data.Common;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler {
    [SerializeField] Image slotImage;
    [SerializeField] TMP_Text slotName;
    [SerializeField] ItemType itemType;
    [SerializeField] Image playerDisplayImage;
    [SerializeField] public GameObject selectedShader;
    [SerializeField] public bool isThisItemSelected;

    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    public Image itemDescriptionImage;

    private ItemSO itemSO;
    private bool slotIsInUse;

    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;

    private void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<EquipmentSOLibrary>(out equipmentSOLibrary)) {
            Debug.LogError("EquipmentSOLibrary not found!");
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
        for (int i = 0; i < equipmentSOLibrary.equipmentSOs.Length; i++) {
            if (equipmentSOLibrary.equipmentSOs[i].itemId == itemSO.itemId) {
                equipmentSOLibrary.equipmentSOs[i].EquipItem();
            }
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
        for (int i = 0; i < equipmentSOLibrary.equipmentSOs.Length; i++) {
            if (equipmentSOLibrary.equipmentSOs[i].itemId == itemSO.itemId) {
                equipmentSOLibrary.equipmentSOs[i].UnEquipItem();
                slotIsInUse = false;
            }
        }
        GameObject.FindWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>().TurnOffPreviewStats();
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
            GameObject.FindWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>().TurnOffPreviewStats();
            // zruš info o itemu
        } else {
            inventoryManager.DeselectAllEquippedSlots();
            selectedShader.SetActive(true);
            isThisItemSelected = true;
            if (slotIsInUse) {
                for (int i = 0; i < equipmentSOLibrary.equipmentSOs.Length; i++) {
                    if (equipmentSOLibrary.equipmentSOs[i].itemId == itemSO.itemId) {
                        equipmentSOLibrary.equipmentSOs[i].PreviewEquipment();
                        // ukaž info o itemu
                    }
                }
                SetPictureAndTextInItemDescription();
            } else {
                GameObject.FindWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>().TurnOffPreviewStats();
            }
        }
    }

    private void SetPictureAndTextInItemDescription() {
        itemDescriptionNameText.text = itemSO.itemName;
        itemDescriptionText.text = itemSO.itemDescription;
        if (ShouldUseSmallIcon()) {
            if (itemSO.iconSprite != null) {
                itemDescriptionImage.sprite = itemSO.iconSprite;
            } else {
                Debug.LogError("Icon sprite not found for " + itemSO.itemName);
                itemDescriptionImage.sprite = itemSO.itemPicture;
            }
        } else {
            itemDescriptionImage.sprite = itemSO.itemPicture;
        }
    }

    private bool ShouldUseSmallIcon() {
        return itemSO.itemType switch {
            ItemType.ammo => true,
            ItemType.consumable => true,
            _ => false,
        };
    }

    private void OnRightClick() {
        UnEquipGear();
    }
}
