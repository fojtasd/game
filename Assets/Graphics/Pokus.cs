using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Pokus : MonoBehaviour {
    private SpriteLibrary spriteLibrary;
    private SpriteResolver spriteResolver;

    public string newCategory;
    public string newLabel;

    public SpriteLibraryAsset legsEquipmentLibraryAsset;
    public SpriteLibraryAsset legsNakedLibraryAsset;

    bool changed = false;

    void Start() {
        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteResolver = GetComponent<SpriteResolver>();
    }

    public void ChangeLegs() {
        if (!changed) {
            spriteLibrary.spriteLibraryAsset = legsEquipmentLibraryAsset;
            changed = true;
        }
        else {
            spriteLibrary.spriteLibraryAsset = legsNakedLibraryAsset;
            changed = false;
        }
    }

    void ChangeSpriteResolver(string category, string label) {
        if (spriteResolver != null) {
            spriteResolver.SetCategoryAndLabel(category, label);
            Debug.Log("Sprite Resolver changed to Category: " + category + ", Label: " + label);
        }
        else {
            Debug.LogWarning("Sprite Resolver is null!");
        }
    }
}
