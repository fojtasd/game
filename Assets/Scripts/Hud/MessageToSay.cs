using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MessageToSay : MonoBehaviour, IPointerClickHandler {
    public static MessageToSay Instance { get; private set; }
    [SerializeField][TextArea] PlayerSoundsLibrary playerSoundsLibrary;
    Coroutine currentCoroutine;
    [SerializeField] TypewriterEffect typewriterEffect;
    [SerializeField] AudioClipName voiceClipName;

    public delegate void TalkingDelegate(bool isTalking);
    public static event TalkingDelegate TalkingEvent;

    TMP_Text text;
    CanvasGroup group;

    bool isTalking = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start() {
        text = GetComponentInChildren<TMP_Text>();
        group = GetComponent<CanvasGroup>();
        typewriterEffect = GetComponentInChildren<TypewriterEffect>();
        playerSoundsLibrary = GameObject.FindWithTag("Settings").GetComponentInChildren<PlayerSoundsLibrary>();
        group.alpha = 0;
    }

    public void SayLine(string line, float duration = 10f, AudioClipName voiceClipName = AudioClipName.None) {
        if (line == "") {
            return;
        }
        if (isTalking) {
            StopCoroutine(currentCoroutine);
            CleanupBubble();
        }
        currentCoroutine = StartCoroutine(SayLineCoroutine(line, duration, voiceClipName));
    }

    IEnumerator SayLineCoroutine(string line, float duration = 5f, AudioClipName voiceClipName = AudioClipName.None) {
        isTalking = true;
        text.SetText(line);
        TalkingEvent?.Invoke(true);
        group.alpha = 1;

        playerSoundsLibrary.PlayAudioClip(voiceClipName);
        typewriterEffect.StartTyping(line);
        while (playerSoundsLibrary.IsPlaying()) {
            yield return null;
        }
        TalkingEvent?.Invoke(false);
        yield return new WaitForSeconds(duration);
        CleanupBubble();
    }

    void CleanupBubble() {
        playerSoundsLibrary.StopPlayingAudioClip();
        TalkingEvent?.Invoke(false);
        typewriterEffect.Skip();
        StopCoroutine(currentCoroutine);
        isTalking = false;
        text.SetText("");
        group.alpha = 0;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            typewriterEffect.Skip();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            CleanupBubble();
        }
    }
}
