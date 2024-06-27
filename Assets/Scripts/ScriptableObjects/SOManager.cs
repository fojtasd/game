using UnityEngine;

public class SOManager : ScriptableObject {
    public ItemSOBase[] itemTemplates;

    public void Setup(HealthManager healthManager, AmmoManager ammoManager) {
        foreach (var itemTemplate in itemTemplates) {
            if (itemTemplate != null) {
                itemTemplate.Setup(healthManager, ammoManager);
            } else {
                Debug.LogWarning("An ItemTemplate is not assigned in the ItemManagerSO.");
            }
        }
    }
}
