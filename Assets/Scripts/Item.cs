using UnityEngine;

public class Item : MonoBehaviour {
    public ItemSO itemSO;
    public int quantity;

    public delegate void PlayerTouched(Item item);
    public static event PlayerTouched OnPlayerTouched;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            OnPlayerTouched?.Invoke(this);
        }
    }

    public ItemId GetItemId() {
        return itemSO.itemId;
    }

    public ItemType GetItemType() {
        return itemSO.itemType;
    }

    public Item Clone() {
        GameObject itemObject = new(itemSO.itemName);
        Item newItem = itemObject.AddComponent<Item>();
        newItem.itemSO = itemSO;
        newItem.quantity = quantity;
        return newItem;
    }

    public string GetItemName() {
        return itemSO.itemName;
    }

    public string GetItemDescription() {
        return itemSO.itemDescription;
    }

    public Sprite GetItemSlotSprite() {
        return itemSO.itemPicture;
    }

    public bool IsEquipment() {
        return itemSO.isEquipment;
    }

    public int GetMaxStoredItemQuantity() {
        return itemSO.maxNumberOfItemsPerSlot;
    }

    public Sprite GetEmptySprite() {
        return itemSO.emptySprite;
    }

    public Sprite GetIconSprite() {
        return itemSO.iconSprite;
    }

    public Sprite GetEquippedItemSprite() {
        return itemSO.equippedItemSprite;
    }
}
