
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChatBubble : MonoBehaviour {
    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake() {
        backgroundSpriteRenderer = transform.Find("BubbleBackground").GetComponent<SpriteRenderer>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Start() {
        Setup("Hello, World!");
    }

    private void Setup(String text) {
        textMeshPro.text = text;
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);





    }

}
