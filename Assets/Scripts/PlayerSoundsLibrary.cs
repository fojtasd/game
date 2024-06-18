using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerSoundsLibrary : MonoBehaviour {
    private Dictionary<string, string> audioClipPaths = new Dictionary<string, string>();
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioClipPaths.Add("a", "SFX/player-voice/a");
        audioClipPaths.Add("b", "SFX/player-voice/b");

        foreach (var kvp in audioClipPaths) {
            AudioClip clip = Resources.Load<AudioClip>(kvp.Value);
            if (clip != null) {
                audioClips.Add(kvp.Key, clip);
            }
            else {
                Debug.LogError("Failed to load audio clip: " + kvp.Value);
            }
        }
    }

    /// <summary>
    /// Displayes message in bubble
    /// </summary>
    /// <param name="name">Name of the clip to play. If empty string (default), no clip is played. If "random" inputed, random clip is played.</param>
    /// <returns>Mothing, its Courutine</returns>
    public void PlayAudioClip(string name = "") {
        if (name == "") {
            return;
        }
        else if (name == "random") {
            PlayRandomClip();
        }
        else {
            audioSource.clip = audioClips[name];
            audioSource.Play();
        }
    }

    private void PlayRandomClip() {
        int randomIndex = Random.Range(0, audioClipPaths.Count);
        string randomKey = audioClipPaths.Keys.ToList().ElementAt(randomIndex);
        PlayAudioClip(randomKey);
    }

    public void StopPlayingAudioClip() {
        audioSource.Stop();
    }

    public bool IsPlaying() {
        if (audioSource.isPlaying) {
            return true;
        }
        return false;
    }
}
