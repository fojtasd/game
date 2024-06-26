using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class ItemSOBase : ScriptableObject {
    protected HealthManager healthManager;
    protected AmmoManager ammoManager;

    public void Setup(HealthManager healthManager, AmmoManager ammoManager) {
        this.healthManager = healthManager;
        this.ammoManager = ammoManager;
    }

    public abstract void UseItem();
}
