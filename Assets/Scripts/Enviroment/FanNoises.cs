using UnityEngine;

public class FanNoises : MonoBehaviour {
    Animator animator;
    readonly string animationName = "ceiling-fan-rotation";
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
        else {
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
        }
    }
}
