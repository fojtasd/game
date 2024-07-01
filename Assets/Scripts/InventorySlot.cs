using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class InventorySlot : MonoBehaviour, IPointerClickHandler {

    // ====== ITEM DATA ======= //
    private Item storedItem;

    // ====== ITEM SLOT ======= //
    [SerializeField] Image itemImage;

    [SerializeField] TMP_Text quantityText;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    public Image itemDescriptionImage;

    public GameObject selectedShader;
    public bool isThisSlotSelected;


    // ====== EQUIPPED SLOTS ======= //
    [SerializeField] EquipmentSlot headSlot, bodySlot, mainHandSlot, shirtSlot, offHandSlot, legsSlot, feetSlot, relicSlot;

    // ====== INVENTORY MANAGER ======= //
    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;

    public delegate void ItemDropped();
    public static event ItemDropped OnItemDropped;


    void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<EquipmentSOLibrary>(out equipmentSOLibrary)) {
            Debug.LogError("EquipmentSOLibrary not found!");
        }
    }

    public Item AddItemToSlot(Item item) {
        if (storedItem == null) {
            storedItem = item.Clone();
            storedItem.quantity = 0;
            return TransferItemQuantity(item);
        }
        if (item.GetItemId() != storedItem.GetItemId()) {
            return item;
        }
        return TransferItemQuantity(item);
    }

    private Item TransferItemQuantity(Item item) {
        int availableSlotQuantity = item.GetMaxStoredItemQuantity() - storedItem.quantity;
        int quantityToStoreInSlot = Math.Min(item.quantity, availableSlotQuantity);
        item.quantity -= quantityToStoreInSlot;
        storedItem.quantity += quantityToStoreInSlot;
        UpdateVisuals();
        if (item.quantity != 0) {
            return item;
        }
        return null;
    }

    public void UpdateVisuals() {
        if (ShouldUseSmallIcon()) {
            if (storedItem.itemSO.iconSprite != null) {
                itemImage.sprite = storedItem.itemSO.iconSprite;
            } else {
                Debug.LogError("Icon sprite not found for " + storedItem.itemSO.itemName);
                itemImage.sprite = storedItem.itemSO.itemPicture;
            }
        } else {
            itemImage.sprite = storedItem.itemSO.itemPicture;
        }
        UpdateText();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnLeftClick();
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            OnRightClick();
        }
    }

    public void OnLeftClick() {
        if (isThisSlotSelected) {
            if (storedItem == null) {
                inventoryManager.DeselectInventorySlots();
                return;
            }
            if (storedItem.IsEquipment()) {
                EquipGear();
                inventoryManager.DeselectInventorySlots();
            } else {
                UseItem();
            }
        } else {
            if (storedItem == null) {
                inventoryManager.DeselectInventorySlots();
                SelectThisSlot();
                return;
            }
            SetItemDescription();
            if (storedItem.IsEquipment()) {
                inventoryManager.DeselectInventorySlots();

                foreach (var equipmentSO in equipmentSOLibrary.equipmentSOs) {
                    if (equipmentSO.itemId == storedItem.itemSO.itemId) {
                        equipmentSO.PreviewEquipment();
                    }
                }
            } else {
                GameObject.FindWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>().TurnOffPreviewStats();
            }

            inventoryManager.DeselectInventorySlots();
            SelectThisSlot();
            if (storedItem.itemSO == null) {
                return;
            }
        }
    }

    public void SelectThisSlot() {
        isThisSlotSelected = true;
        selectedShader.SetActive(true);
    }

    public void OnRightClick() {
        if (storedItem == null) {
            return;
        }
        DropItem();
    }

    public void DropItem() {
        GameObject itemToDrop = new(this.storedItem.itemSO.itemName);
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.quantity = storedItem.quantity;
        newItem.itemSO = storedItem.itemSO;

        SpriteRenderer spriteRenderer = itemToDrop.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = storedItem.itemSO.itemPicture;
        spriteRenderer.sortingOrder = 0;
        spriteRenderer.sortingLayerName = "Player";

        itemToDrop.layer = LayerMask.NameToLayer("Items");
        itemToDrop.AddComponent<BoxCollider2D>();
        itemToDrop.AddComponent<Rigidbody2D>();
        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(0.5f, 0, 0);

        EmptySlot();
    }

    public void EmptySlot() {
        itemImage.sprite = storedItem.GetEmptySprite();
        quantityText.enabled = false;
        GameObject originalItemObject = storedItem.gameObject;
        Destroy(originalItemObject);
        storedItem = null;
        OnItemDropped?.Invoke();
    }

    private void UseItem() {
        if (storedItem == null) {
            return;
        }

        bool wasItemUsed = inventoryManager.UseItem(storedItem.GetItemId());
        ResolveItemUsage(wasItemUsed);
    }

    private void ResolveItemUsage(bool wasItemUsed) {
        if (wasItemUsed) {
            storedItem.quantity--;
            UpdateText();
            if (storedItem.quantity == 0) {
                inventoryManager.DeselectInventorySlots();
                EmptySlot();
            } else {
                selectedShader.SetActive(true);
                isThisSlotSelected = true;
                if (storedItem == null) {
                    return;
                }
                SetItemDescription();
            }
        }
    }

    private void UpdateText() {
        if (storedItem.quantity == 1) {
            quantityText.text = "";
        } else {
            quantityText.text = storedItem.quantity.ToString();
        }
        quantityText.enabled = true;
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
        return storedItem.GetItemType() switch {
            ItemType.ammo => true,
            ItemType.consumable => true,
            _ => false,
        };
    }

    private void EquipGear() {
        switch (storedItem.GetItemType()) {
            case ItemType.head:
                headSlot.EquipGear(storedItem);
                break;
            case ItemType.shirt:
                shirtSlot.EquipGear(storedItem);
                break;
            case ItemType.body:
                bodySlot.EquipGear(storedItem);
                break;
            case ItemType.mainHand:
                mainHandSlot.EquipGear(storedItem);
                break;
            case ItemType.offHand:
                offHandSlot.EquipGear(storedItem);
                break;
            case ItemType.legs:
                legsSlot.EquipGear(storedItem);
                break;
            case ItemType.feet:
                feetSlot.EquipGear(storedItem);
                break;
            case ItemType.relic:
                relicSlot.EquipGear(storedItem);
                break;
        }
        EmptySlot();
    }

    public bool IsSlotFree() {
        return storedItem == null;
    }

    public Item GetStoredItem() {
        return storedItem;
    }

    public void SetStoredItem(Item item) {
        storedItem = item;
    }
}