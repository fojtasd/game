using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour {
    AmmoManager ammoManager;
    TextMeshProUGUI ammoText;

    public void Awake() {
        ammoText = GameObject.FindWithTag("AmmoBar").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(AmmoManager ammoManager) {
        this.ammoManager = ammoManager;
    }

    void Update() {
        ammoText.text = ammoManager.GetCurrentAmountOfAmmo().ToString();
    }
}
