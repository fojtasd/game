using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSoundsLibrary : MonoBehaviour {
    readonly Dictionary<AudioClipName, string> audioClipPaths = new();
    readonly Dictionary<AudioClipName, AudioClip> audioClips = new();
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioClipPaths.Add(AudioClipName.ClipA, "SFX/player-voice/a");
        audioClipPaths.Add(AudioClipName.ClipB, "SFX/player-voice/b");

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

    public void PlayAudioClip(AudioClipName clipName) {
        if (clipName == AudioClipName.None) {
            return;
        }
        else if (clipName == AudioClipName.Random) {
            PlayRandomClip();
        }
        else {
            audioSource.clip = audioClips[clipName];
            audioSource.Play();
        }
    }

    public void StopPlayingAudioClip() {
        audioSource.Stop();
    }

    public bool IsPlaying() {
        return audioSource.isPlaying;
    }

    void PlayRandomClip() {
        int randomIndex = Random.Range(0, audioClipPaths.Count);
        AudioClipName randomKey = audioClipPaths.Keys.ToList().ElementAt(randomIndex);
        PlayAudioClip(randomKey);
    }
}
