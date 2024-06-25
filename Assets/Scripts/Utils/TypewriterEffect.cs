using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypewriterEffect : MonoBehaviour {
    [SerializeField] private float charactersPerSecond = 20f;
    [SerializeField] private float punctuationDelay = 0.5f;

    private TextMeshProUGUI _textBox;
    private Coroutine _typewriterCoroutine;
    private WaitForSeconds _normalDelay;
    private WaitForSeconds _punctuationDelay;

    private void Awake() {
        _textBox = GetComponent<TextMeshProUGUI>();
        _normalDelay = new WaitForSeconds(1f / charactersPerSecond);
        _punctuationDelay = new WaitForSeconds(punctuationDelay);
        _textBox.maxVisibleCharacters = 0;
    }

    public void StartTyping(string text) {
        _textBox.text = text;
        _textBox.maxVisibleCharacters = 0;

        if (_typewriterCoroutine != null) {
            StopCoroutine(_typewriterCoroutine);
        }

        _typewriterCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText() {
        _textBox.ForceMeshUpdate();
        int totalCharacters = _textBox.textInfo.characterCount;
        int visibleCount = 1;

        while (visibleCount <= totalCharacters) {
            _textBox.maxVisibleCharacters = visibleCount;

            char currentChar = _textBox.text[visibleCount - 1];
            visibleCount++;

            if (IsPunctuation(currentChar)) {
                yield return _punctuationDelay;
            } else {
                yield return _normalDelay;
            }
        }
        _textBox.maxVisibleCharacters = totalCharacters;
    }

    private bool IsPunctuation(char character) {
        return character == '.' || character == ',' || character == '!' || character == '?' || character == ';' || character == ':';
    }

    public void Skip() {
        if (_typewriterCoroutine != null) {
            StopCoroutine(_typewriterCoroutine);
            _textBox.maxVisibleCharacters = _textBox.text.Length;
        }
    }
}