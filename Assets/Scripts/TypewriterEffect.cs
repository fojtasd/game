using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;


[RequireComponent(typeof(TextMeshProUGUI))]
public class TypewriterEffect : MonoBehaviour {
    private TextMeshProUGUI _textBox;
    private string current_text;

    // Basic Typewriter Functionality
    private int _currentVisibleCharacterIndex;
    private Coroutine _typewriterCoroutine;
    private bool _readyForNewText = true;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.5f;


    // Skipping Functionality
    public bool CurrentlySkipping { get; private set; }
    private WaitForSeconds _skipDelay;

    [Header("Skip options")]
    [SerializeField] private bool quickSkip;
    [SerializeField][Min(1)] private int skipSpeedup = 5;


    // Event Functionality
    private WaitForSeconds _textboxFullEventDelay;
    [SerializeField][Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f; // In testing, I found 0.25 to be a good value

    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;


    private void Awake() {
        _textBox = GetComponent<TextMeshProUGUI>();
        _textBox.maxVisibleCharacters = 0;

        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));
        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }

    private void OnEnable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    }

    private void OnDisable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    }

    #region Skipfunctionality
    // Example for how to implement it in the New Input system
    // You'd have to use a PlayerController component on this gameobject and write the function's name as On[Input Action name] for this to work.
    // In this case, my Input Action is called "RightMouseClick". But: There are a ton of ways to implement checking if a button
    // has been pressed in this system. Go watch Piti's video on the different ways of utilizing the new input system: https://www.youtube.com/watch?v=Wo6TarvTL5Q

    // private void OnRightMouseClick()
    // {
    //     if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
    //         Skip();
    // }
    #endregion

    private void PrepareForNewText(Object obj) {
        if (obj != _textBox) {
            return;
        }

        if (!_readyForNewText) {
            return;
        }

        if (_textBox.maxVisibleCharacters > _textBox.textInfo.characterCount) {
            return;
        }

        if (_textBox.text == current_text) {
            return;
        }

        CurrentlySkipping = false;
        _readyForNewText = false;

        if (_typewriterCoroutine != null) {
            StopCoroutine(_typewriterCoroutine);
        }

        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter() {
        TMP_TextInfo textInfo = _textBox.textInfo;

        while (_currentVisibleCharacterIndex < textInfo.characterCount + 1) {
            var lastCharacterIndex = textInfo.characterCount - 1;

            if (_currentVisibleCharacterIndex >= lastCharacterIndex) {
                _textBox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                _readyForNewText = true;
                yield break;
            }

            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

            _textBox.maxVisibleCharacters++;

            if (!CurrentlySkipping &&
                (character == '?' || character == '.' || character == ',' || character == ':' ||
                 character == ';' || character == '!' || character == '-')) {
                yield return _interpunctuationDelay;
            }
            else {
                yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
            }

            CharacterRevealed?.Invoke(character);
            _currentVisibleCharacterIndex++;
        }
    }

    public void Skip(bool quickSkipNeeded = false) {
        if (IsPointerOverUIObject()) {
            Debug.Log("Click detected");
            if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1) {
                if (CurrentlySkipping)
                    return;

                CurrentlySkipping = true;




                StopCoroutine(_typewriterCoroutine);
                _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
                _readyForNewText = true;
                CompleteTextRevealed?.Invoke();
            }
        }

    }

    private IEnumerator SkipSpeedupReset() {
        yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
        CurrentlySkipping = false;
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}