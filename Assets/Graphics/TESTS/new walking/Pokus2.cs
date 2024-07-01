using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Pokus2 : MonoBehaviour {
    private SpriteLibrary spriteLibrary;
    private SpriteResolver spriteResolver;
    bool changed = false;

    public SpriteLibraryAsset feetEquipmentLibraryAsset;
    public SpriteLibraryAsset feetNakedLibraryAsset;

    void Start() {
        spriteLibrary = GetComponent<SpriteLibrary>();
        spriteResolver = GetComponent<SpriteResolver>();
    }

    public void ChangeLegs() {
        if (!changed) {

            spriteResolver.SetCategoryAndLabel("Walking", spriteResolver.GetLabel());
            spriteLibrary.spriteLibraryAsset = feetEquipmentLibraryAsset;
            changed = true;
        }
        else {
            spriteLibrary.spriteLibraryAsset = feetNakedLibraryAsset;
            spriteResolver.SetCategoryAndLabel("Walking", spriteResolver.GetLabel());
            changed = false;
        }
    }
}
