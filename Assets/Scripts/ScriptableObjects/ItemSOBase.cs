using UnityEngine;

public abstract class ItemSOBase : ScriptableObject {
    protected HealthManager healthManager;
    protected AmmoManager ammoManager;
    public Sprite emptySprite;
    public Sprite iconSprite;
    public Sprite equippedItemSprite;

    public void Setup(HealthManager healthManager, AmmoManager ammoManager) {
        this.healthManager = healthManager;
        this.ammoManager = ammoManager;
        emptySprite = Resources.Load<Sprite>("Sprites/emptySprite");
    }

    public abstract bool UseItem();
}
