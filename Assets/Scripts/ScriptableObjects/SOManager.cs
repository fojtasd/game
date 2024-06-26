using UnityEngine;

[CreateAssetMenu(fileName = "SOManager", menuName = "ScriptableObjects/SOManager")]
public class SOManager : ScriptableObject {
    public ItemSOBase[] itemTemplates;

    public void Setup(HealthManager healthMgr, AmmoManager ammoMgr) {
        foreach (var itemTemplate in itemTemplates) {
            if (itemTemplate != null) {
                itemTemplate.Setup(healthMgr, ammoMgr);
            }
            else {
                Debug.LogWarning("An ItemTemplate is not assigned in the ItemManagerSO.");
            }
        }
    }
}
