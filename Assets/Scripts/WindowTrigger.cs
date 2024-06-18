using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTrigger : MonoBehaviour {
    public Animator windowAnimator;
    private bool isInside = false;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isInside) {
            windowAnimator.SetBool("isOpening", true);
            windowAnimator.SetBool("isClosing", false);
            PlaySound();
            isInside = true;
        }
        else if (other.CompareTag("Player") && isInside) {
            windowAnimator.SetBool("isOpening", false);
            windowAnimator.SetBool("isClosing", true);
            PlaySound();
            isInside = false;
        }
    }

    public void PlaySound() {
        if (audioSource != null) {
            audioSource.Play();
        }
    }
}
