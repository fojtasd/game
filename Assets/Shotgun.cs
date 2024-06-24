using UnityEngine;

public class Shotgun : MonoBehaviour {
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            Debug.LogError("No AudioSource found on Shotgun GameObject.");
        }
    }

    public void Shoot() {
        if (audioSource != null && audioSource.clip != null) {
            audioSource.Play();
        }
        else {
            Debug.LogError("AudioSource or AudioClip is missing.");
        }
    }
}
