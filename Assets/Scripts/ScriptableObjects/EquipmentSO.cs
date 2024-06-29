using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject {
    public ItemId itemId;
    public int attack, defense;
    [SerializeField] private Sprite sprite;
    // find out which attributes I want to have

    public void PreviewEquipment() {
        GameObject.FindWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>().PreviewEquipmentStats(attack, defense, sprite);
    }

    public void EquipItem() {
        PlayerStatsManager playerStatsManager = GameObject.FindGameObjectWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>();
        playerStatsManager.attack += attack;
        playerStatsManager.defense += defense;

        playerStatsManager.UpdateEquipmentStats();
    }

    public void UnEquipItem() {
        // update stats
        PlayerStatsManager playerStatsManager = GameObject.FindGameObjectWithTag("PlayerStatsManager").GetComponent<PlayerStatsManager>();
        playerStatsManager.attack -= attack;
        playerStatsManager.defense -= defense;
        playerStatsManager.UpdateEquipmentStats();
    }

}
