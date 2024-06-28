using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler {

    // ====== ITEM DATA ======= //
    private ItemSO itemSO;
    public int quantity;

    // ====== ITEM SLOT ======= //
    [SerializeField] Image itemImage;
    public GameObject selectedShader;
    public bool isThisItemSelected;
    public bool isFull;


    // ====== EQUIPPED SLOTS ======= //
    [SerializeField] EquippedSlot headSlot, bodySlot, mainHandSlot, shirtSlot, offHandSlot, legsSlot, feetSlot, relicSlot;



    // ====== INVENTORY MANAGER ======= //
    private InventoryManager inventoryManager;


    void Awake() {
        if (!GameObject.FindWithTag("InventoryCanvas").TryGetComponent<InventoryManager>(out inventoryManager)) {
            Debug.LogError("InventoryManager not found");
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
        this.quantity = 1;
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
        if (isThisItemSelected) {
            if (itemSO == null) {
                return;
            }
            EquipGear();

        } else {
            inventoryManager.DeselectAllEquipmentSlots();
            selectedShader.SetActive(true);
            isThisItemSelected = true;
            if (itemSO == null) {
                return;
            }
        }
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
        itemSO = null;
        isFull = false;
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
}
