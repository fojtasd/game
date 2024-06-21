using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleDialogBox : MonoBehaviour, IPointerClickHandler {
    [SerializeField]
    [TextArea]
    private List<string> _dialogLines;
    private PlayerSoundsLibrary playerSoundsLibrary;
    private Animator _headAnimator;
    private Coroutine currentCoroutine;
    [SerializeField] private TypewriterEffect typewriterEffect;

    private TMP_Text _text;
    private CanvasGroup _group;

    private bool isTalking = false;

    private void Start() {
        _text = GetComponentInChildren<TMP_Text>();
        _group = GetComponent<CanvasGroup>();
        typewriterEffect = GetComponentInChildren<TypewriterEffect>();
        playerSoundsLibrary = GameObject.FindWithTag("Settings").GetComponentInChildren<PlayerSoundsLibrary>();
        _headAnimator = GameObject.Find("HudCanvas").transform.Find("HudPanel/HeadAnimator").gameObject.GetComponent<Animator>();
        _group.alpha = 0;
    }

    public void SayLine(string line, float duration = 10f, string voiceClipName = "") {
        if (isTalking) {
            StopCoroutine(currentCoroutine);
            CleanupBubble();
        }
        currentCoroutine = StartCoroutine(SayLineCoroutine(line, duration, voiceClipName));
    }

    /// <summary>
    /// Displayes message in bubble
    /// </summary>
    /// <param name="line">Message to say</param>
    /// <param name="duration">How long should message be visible</param>
    /// <param name="voiceClipName">Name of the clip to play. If empty string (default), no clip. If "random" inputed, random clip is played.</param>
    /// <returns>Mothing, its Courutine</returns>
    private IEnumerator SayLineCoroutine(string line, float duration = 5f, string voiceClipName = "") {
        isTalking = true;
        _text.SetText(line);
        _headAnimator.SetBool("isTalking", true);
        _headAnimator.SetInteger("priority", 3);
        _group.alpha = 1;

        playerSoundsLibrary.PlayAudioClip(voiceClipName);
        StartCoroutine(SetRandomTalkingFace());
        typewriterEffect.StartTyping(line);
        yield return new WaitForSeconds(duration);

        CleanupBubble();
    }

    private IEnumerator SetRandomTalkingFace() {
        while (isTalking && playerSoundsLibrary.IsPlaying()) {
            int randomInt = Random.Range(0, 3);
            _headAnimator.SetInteger("randomTalkingAnimation", randomInt);
            yield return new WaitForSeconds(0.5f);
        }
        _headAnimator.SetBool("isTalking", false);
        _headAnimator.SetInteger("priority", 1);
    }

    private void CleanupBubble() {
        playerSoundsLibrary.StopPlayingAudioClip();
        typewriterEffect.Skip();
        StopCoroutine(currentCoroutine);
        isTalking = false;
        _text.SetText("");
        _group.alpha = 0;
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
