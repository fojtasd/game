using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler {
    [SerializeField] Image slotImage;
    [SerializeField] TMP_Text slotName;
    [SerializeField] ItemType itemType;
    [SerializeField] Image playerDisplayImage;
    [SerializeField] public GameObject selectedShader;
    [SerializeField] public bool isThisSlotSelected;

    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    public Image itemDescriptionImage;

    private Item storedItem;

    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;
    private PlayerStatsManager playerStatsManager;

    private void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<EquipmentSOLibrary>(out equipmentSOLibrary)) {
            Debug.LogError("EquipmentSOLibrary not found!");
        }
        if (!GameObject.FindWithTag("PlayerStatsManager").TryGetComponent<PlayerStatsManager>(out playerStatsManager)) {
            Debug.LogError("PlayerStatManager not found!");
        }
    }

    public bool IsSlotFree() {
        return storedItem == null;
    }

    public void EquipGear(Item item) {
        if (IsSlotFree()) {
            UnEquipGear();
        }
        storedItem = item.Clone();
        slotImage.sprite = item.GetItemSlotSprite();
        slotName.enabled = false;
        UpdateAvatarVisuals();


        for (int i = 0; i < equipmentSOLibrary.equipmentSOs.Length; i++) {
            if (equipmentSOLibrary.equipmentSOs[i].itemId == item.itemSO.itemId) {
                equipmentSOLibrary.equipmentSOs[i].EquipItem();
            }
        }
    }

    public void UpdateAvatarVisuals() {
        if (storedItem.GetEquippedItemSprite() != null) {
            playerDisplayImage.sprite = storedItem.GetEquippedItemSprite();
        } else {
            playerDisplayImage.sprite = storedItem.GetItemSlotSprite();
        }
    }

    public void UnEquipGear() {
        if (IsSlotFree()) {
            return;
        }
        inventoryManager.DeselectAllEquipmentSlots();
        inventoryManager.AddItemToInventory(storedItem);
        slotImage.sprite = storedItem.GetEmptySprite();
        slotName.enabled = true;
        playerDisplayImage.sprite = storedItem.GetEmptySprite();
        for (int i = 0; i < equipmentSOLibrary.equipmentSOs.Length; i++) {
            if (equipmentSOLibrary.equipmentSOs[i].itemId == storedItem.itemSO.itemId) {
                equipmentSOLibrary.equipmentSOs[i].UnEquipItem();
            }
        }
        playerStatsManager.TurnOffPreviewStats();
        storedItem.itemSO = null;
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
        if (storedItem == null) {
            return;
        }
        if (isThisSlotSelected) {
            UnEquipGear();
            playerStatsManager.TurnOffPreviewStats();
            // zruš info o itemu
        } else {
            inventoryManager.DeselectAllEquipmentSlots();
            SelectThisSlot();
            if (!IsSlotFree()) {
                for (int i = 0; i < equipmentSOLibrary.equipmentSOs.Length; i++) {
                    if (equipmentSOLibrary.equipmentSOs[i].itemId == storedItem.GetItemId()) {
                        equipmentSOLibrary.equipmentSOs[i].PreviewEquipment();
                    }
                }
                SetItemDescription();
            } else {
                playerStatsManager.TurnOffPreviewStats();
            }
        }
    }

    public void SelectThisSlot() {
        isThisSlotSelected = true;
        selectedShader.SetActive(true);
    }

    private void SetItemDescription() {
        itemDescriptionNameText.text = storedItem.GetItemName();
        itemDescriptionText.text = storedItem.GetItemDescription();
        if (ShouldUseSmallIcon()) {
            if (storedItem.GetIconSprite() != null) {
                itemDescriptionImage.sprite = storedItem.GetIconSprite();
            } else {
                Debug.LogError("Icon sprite not found for " + storedItem.GetItemName());
                itemDescriptionImage.sprite = storedItem.GetItemSlotSprite();
            }
        } else {
            itemDescriptionImage.sprite = storedItem.itemSO.itemPicture;
        }
        itemDescriptionText.enabled = true;
        itemDescriptionNameText.enabled = true;
    }

    private bool ShouldUseSmallIcon() {
        return storedItem.itemSO.itemType switch {
            ItemType.ammo => true,
            ItemType.consumable => true,
            _ => false,
        };
    }
    private void OnRightClick() {
        if (storedItem == null) {
            return;
        }
        UnEquipGear();
    }

    public void EmptySlot() {
        slotImage.sprite = storedItem.GetEmptySprite();
        slotName.enabled = true;
        GameObject originalItemObject = storedItem.gameObject;
        Destroy(originalItemObject);
        storedItem = null;
    }
}
