using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler {

    // ====== ITEM DATA ======= //
    private ItemSO itemSO;
    public int quantity;

    // ====== ITEM SLOT ======= //
    [SerializeField] Image itemImage;





    [SerializeField] TMP_Text quantityText;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    public Image itemDescriptionImage;






    public GameObject selectedShader;
    public bool isThisItemSelected;
    public bool isFull;


    // ====== EQUIPPED SLOTS ======= //
    [SerializeField] EquippedSlot headSlot, bodySlot, mainHandSlot, shirtSlot, offHandSlot, legsSlot, feetSlot, relicSlot;



    // ====== INVENTORY MANAGER ======= //
    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;


    void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
        }
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<EquipmentSOLibrary>(out equipmentSOLibrary)) {
            Debug.LogError("EquipmentSOLibrary not found!");
        }
        isFull = false;
    }

    public int AddItemToSlot(int quantity, ItemSO itemSO) {
        if (isFull) {
            return quantity;
        }
        this.itemSO = itemSO;
        if (ShouldUseSmallIcon()) {
            if (itemSO.iconSprite != null) {
                itemImage.sprite = itemSO.iconSprite;
            } else {
                Debug.LogError("Icon sprite not found for " + itemSO.itemName);
                itemImage.sprite = itemSO.itemPicture;
            }
        } else {
            itemImage.sprite = itemSO.itemPicture;
        }

        //update quantity
        this.quantity += quantity;
        int maxNumberOfItemsOfThisType = itemSO.maxNumberOfItemsPerSlot;
        if (this.quantity >= maxNumberOfItemsOfThisType) {
            if (this.quantity == 1) {
                quantityText.text = "";
            } else {
                quantityText.text = maxNumberOfItemsOfThisType.ToString();
            }
            quantityText.enabled = true;
            isFull = true;
            int extraItems = this.quantity - maxNumberOfItemsOfThisType;
            this.quantity = maxNumberOfItemsOfThisType;
            return extraItems;
        }
        if (this.quantity == 1) {
            quantityText.text = "";
        } else {
            quantityText.text = quantity.ToString();
        }

        quantityText.enabled = true;
        isFull = true;
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
        if (itemSO == null) {
            return;
        }
        if (isThisItemSelected) {
            if (itemSO == null) {
                return;
            }
            if (itemSO.isEquipment) {
                EquipGear();
                inventoryManager.DeselectAllEquipmentSlots();
            } else {
                UseItem();
            }
        } else {
            SetPictureAndTextInItemDescription();
            if (itemSO.isEquipment) {
                inventoryManager.DeselectAllEquipmentSlots();
            }
            // SHOW STATS PREVIEW
            if (itemSO.isEquipment) {
                for (int i = 0; i < equipmentSOLibrary.equipmentSOs.Length; i++) {
                    if (equipmentSOLibrary.equipmentSOs[i].itemId == itemSO.itemId) {
                        equipmentSOLibrary.equipmentSOs[i].PreviewEquipment();
                    }
                }
            } else {
                GameObject.FindWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>().TurnOffPreviewStats();
            }

            inventoryManager.DeselectAllEquipmentSlots();
            selectedShader.SetActive(true);
            isThisItemSelected = true;
            if (itemSO == null) {
                return;
            }
        }
    }

    public void OnRightClick() {
        if (itemSO == null) {
            return;
        }
        GameObject itemToDrop = new();
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.quantity = 1;
        newItem.itemSO = itemSO;

        SpriteRenderer spriteRenderer = itemToDrop.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemSO.itemPicture;
        spriteRenderer.sortingOrder = 0;
        spriteRenderer.sortingLayerName = "Player";
        itemToDrop.layer = LayerMask.NameToLayer("Items");

        itemToDrop.AddComponent<BoxCollider2D>();
        itemToDrop.AddComponent<Rigidbody2D>();
        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(0.5f, 0, 0);

        quantity--;
        isFull = false;
        if (quantity == 0) {
            EmptySlot();
        }
    }

    public void EmptySlot() {
        itemImage.sprite = itemSO.emptySprite;
        quantityText.text = "";
        quantity = 0;
        itemSO = null;
        isFull = false;
    }

    private void UseItem() {
        if (itemSO == null) {
            return;
        }
        bool wasItemUsed = inventoryManager.UseItem(itemSO.itemId);

        if (wasItemUsed) {
            quantity--;
            isFull = false;
            quantityText.text = quantity.ToString();
            if (quantity == 0) {
                inventoryManager.DeselectAllEquipmentSlots();
                EmptySlot();
            } else {
                selectedShader.SetActive(true);
                isThisItemSelected = true;
                if (itemSO == null) {
                    return;
                }
                SetPictureAndTextInItemDescription();
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

    public ItemSO GetItemSO() {
        return itemSO;
    }

    private bool ShouldUseSmallIcon() {
        return itemSO.itemType switch {
            ItemType.ammo => true,
            ItemType.consumable => true,
            _ => false,
        };
    }

    private void EquipGear() {
        switch (itemSO.itemType) {
            case ItemType.head:
                headSlot.EquipGear(itemSO);
                break;
            case ItemType.shirt:
                shirtSlot.EquipGear(itemSO);
                break;
            case ItemType.body:
                bodySlot.EquipGear(itemSO);
                break;
            case ItemType.mainHand:
                mainHandSlot.EquipGear(itemSO);
                break;
            case ItemType.offHand:
                offHandSlot.EquipGear(itemSO);
                break;
            case ItemType.legs:
                legsSlot.EquipGear(itemSO);
                break;
            case ItemType.feet:
                feetSlot.EquipGear(itemSO);
                break;
            case ItemType.relic:
                relicSlot.EquipGear(itemSO);
                break;
        }
        EmptySlot();
    }
}