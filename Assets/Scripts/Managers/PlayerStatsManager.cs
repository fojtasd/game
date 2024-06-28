using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour {
    public int stamina;
    public int health;

    [SerializeField] private TMP_Text staminaText, healthText;

    private HealthManager healthManager;


    // Start is called before the first frame update
    public void Setup(HealthManager healthManager) {
        this.healthManager = healthManager;
    }

    // Update is called once per frame
    void Update() {
        UpdateEquipmentStats();
    }

    public void UpdateEquipmentStats() {
        staminaText.text = stamina.ToString();
        healthText.text = healthManager.GetHealth().ToString() + "%";
    }
}
