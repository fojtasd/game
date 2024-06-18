using UnityEngine;

public class FanNoises : MonoBehaviour {
    public Animator animator; // Animátor, který spouští animaci
    private string animationName = "ceiling-fan-rotation"; // Název animace, kterou chcete sledovat
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        // Zkontroluje, zda je animace spuštěna
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
