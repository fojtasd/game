using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour {
    // health is solved by HealthManager
    public int attack, defense = 0;

    [SerializeField] TMP_Text defenseText, healthText, attackText;

    [SerializeField] TMP_Text attackPreText, defensePreText;
    [SerializeField] private Image previewImage;

    [SerializeField] GameObject selectedItemStats;
    [SerializeField] GameObject selectedItemImage;

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
        defenseText.text = defense.ToString();
        attackText.text = attack.ToString();
        healthText.text = healthManager.GetHealth().ToString() + "%";
    }

    public void PreviewEquipmentStats(int attack, int defense, Sprite sprite) {
        attackPreText.text = attack.ToString();
        defensePreText.text = defense.ToString();
        previewImage.sprite = sprite;

        selectedItemImage.SetActive(true);
        selectedItemStats.SetActive(true);
    }

    public void TurnOffPreviewStats() {
        selectedItemImage.SetActive(false);
        selectedItemStats.SetActive(false);
    }
}
