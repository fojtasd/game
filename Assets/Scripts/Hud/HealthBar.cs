using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    HealthManager healthManager;
    TextMeshProUGUI hpText;

    public void Awake() {
        hpText = GameObject.FindWithTag("HealthBar").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(HealthManager healthManager) {
        this.healthManager = healthManager;
    }

    void Update() {
        hpText.text = healthManager.GetHealth().ToString() + "%";

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            healthManager.Heal(10);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            healthManager.TakeDamage(10);
        }
    }
}
