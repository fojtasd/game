using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro.Examples;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {

    // ====== ITEM DATA ======= //
    private ItemSO itemSO;
    public int quantity;

    // ====== ITEM SLOT ======= //
    [SerializeField] TMP_Text quantityText;
    [SerializeField] Image itemImage;

    public GameObject selectedShader;
    public bool isThisItemSelected;

    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    public bool isFull;

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
        itemImage.sprite = itemSO.itemPicture;

        //update quantity
        this.quantity += quantity;
        int maxNumberOfItemsOfThisType = itemSO.maxNumberOfItemsPerSlot;
        if (this.quantity >= maxNumberOfItemsOfThisType) {
            quantityText.text = maxNumberOfItemsOfThisType.ToString();
            quantityText.enabled = true;
            isFull = true;
            int extraItems = this.quantity - maxNumberOfItemsOfThisType;
            this.quantity = maxNumberOfItemsOfThisType;
            return extraItems;
        }

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
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
            bool usable = inventoryManager.UseItem(itemSO.itemType);
            if (usable) {
                quantity--;
                isFull = false;
                quantityText.text = quantity.ToString();
                if (quantity == 0) {
                    EmptySlot();
                }
            }
        } else {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            isThisItemSelected = true;
            if (itemSO == null) {
                return;
            }
            itemDescriptionNameText.text = itemSO.itemName;
            itemDescriptionText.text = itemSO.itemDescription;
            itemDescriptionImage.sprite = itemSO.itemPicture;
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
        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(0.5f, 0, 0);

        quantity--;
        isFull = false;
        quantityText.text = quantity.ToString();
        if (quantity == 0) {
            EmptySlot();
        }
    }

    public void EmptySlot() {
        quantityText.enabled = false;
        itemImage.sprite = itemSO.emptySprite;
        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = itemSO.emptySprite;
        itemSO = null;
    }

    public ItemSO GetItemSO() {
        return itemSO;
    }
}
