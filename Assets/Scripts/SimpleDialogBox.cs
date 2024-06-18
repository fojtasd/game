using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleDialogBox : MonoBehaviour {
    [SerializeField]
    [TextArea]
    private List<string> _dialogLines;
    private PlayerSoundsLibrary playerSoundsLibrary;
    private Animator _headAnimator;
    private Coroutine currentCoroutine;

    private TMP_Text _text;
    private CanvasGroup _group;

    private bool isTalking = false;
    private bool skipBubble = false;

    private void Start() {
        _text = GetComponentInChildren<TMP_Text>();
        _group = GetComponent<CanvasGroup>();
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
    private IEnumerator SayLineCoroutine(string line, float duration = 10f, string voiceClipName = "") {
        isTalking = true;
        _text.SetText(line);
        _headAnimator.SetBool("isTalking", true);
        _headAnimator.SetInteger("priority", 3);
        _group.alpha = 1;

        playerSoundsLibrary.PlayAudioClip(voiceClipName);
        StartCoroutine(SetRandomTalkingFace());
        float elapsedTime = 0f;
        while (elapsedTime < duration && !skipBubble) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

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

    public void SkipBubble() {
        skipBubble = true;
    }

    private void CleanupBubble() {
        playerSoundsLibrary.StopPlayingAudioClip();
        isTalking = false;
        skipBubble = false;
        _group.alpha = 0;
    }
}
