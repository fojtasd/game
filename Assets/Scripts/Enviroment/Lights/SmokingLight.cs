using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SmokingLight : MonoBehaviour {

    public Light2D cigaretteLight;

    void Start() {
        cigaretteLight.enabled = false;
    }

    public void TurnCigaretteLightOn() {
        cigaretteLight.enabled = true;
    }

    public void TurnCigaretteLightOff() {
        cigaretteLight.enabled = false;
    }
}
