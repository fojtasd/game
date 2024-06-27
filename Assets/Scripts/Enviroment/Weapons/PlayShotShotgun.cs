using UnityEngine;

public class Shotgun : MonoBehaviour {
    AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            Debug.LogError("No AudioSource found on Shotgun GameObject.");
        }
    }

    // Used in event in animation, do not delete
    void Shoot() {
        if (audioSource != null && audioSource.clip != null) {
            audioSource.Play();
        } else {
            Debug.LogError("AudioSource or AudioClip is missing.");
        }
    }
}
