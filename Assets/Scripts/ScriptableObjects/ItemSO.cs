using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ItemSOBase {

    public string itemId;
    public string itemName;
    public StatToChange statToChange = new();
    public int amountToChangeStat;

    public AttributesToChange attributeToChange = new();
    public int amountToChangeAttribute;

    public override void UseItem() {
        switch (statToChange) {
            case StatToChange.health:
                healthManager.Heal(amountToChangeStat);
                break;
            case StatToChange.ammo:
                ammoManager.AddAmmo(amountToChangeStat);
                break;
            default:
                break;
        }
    }


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
}
