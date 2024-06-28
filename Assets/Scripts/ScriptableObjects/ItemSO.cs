using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ItemSOBase {

    [Header("Basic Info")]
    public ItemId itemId;
    public string itemName;
    [TextArea] public string itemDescription;
    public ItemType itemType;

    [Header("Stat Changes")]
    public StatToChange statToChange = new();
    public int amountToChangeStat;
    public AttributesToChange attributeToChange = new();
    public int amountToChangeAttribute;

    [Header("Visual and Audio")]
    public Sprite itemPicture;
    public AudioClip pickupSound;

    [Header("Limits")]
    public int maxNumberOfItemsPerSlot;

    public enum StatToChange {
        none,
        health,
        ammo
    };

    public enum AttributesToChange {
        none,
        health,
        ammo
    };


    public override bool UseItem() {
        switch (statToChange) {
            case StatToChange.health:
                if (healthManager.GetHealth() == healthManager.GetMaxHealth()) {
                    return false;
                } else {
                    healthManager.Heal(amountToChangeStat);
                    return true;
                }
            case StatToChange.ammo:
                if (ammoManager.GetCurrentAmountOfAmmo() == ammoManager.GetMaxAmmo()) {
                    return false;
                } else {
                    ammoManager.AddAmmo(amountToChangeStat);
                    return true;
                }
            default:
                return false;
        }
    }

    private void OnValidate() {
        if (maxNumberOfItemsPerSlot <= 0) {
            Debug.LogWarning($"maxNumberOfItemsPerSlot for item '{itemName}' must be greater than 0. Current value: {maxNumberOfItemsPerSlot}, otherwise code gets to recursive loop.");
        }
    }

}
