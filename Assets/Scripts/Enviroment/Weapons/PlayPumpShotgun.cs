using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPumpShotgun : MonoBehaviour {
    // Start is called before the first frame update
    AudioSource audioSource;
    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayAudio() {
        audioSource.Play();
    }
}
