using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    void AnimatorPlayStep() {
        audioSource.Play();
    }

    public void PlayLine() {
        audioSource.Play();
    }

    public void StopPlaying() {
        audioSource.Stop();
    }
}
