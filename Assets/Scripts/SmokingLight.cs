using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SmokingLight : MonoBehaviour {

    public Light2D cigaretteLight;

    // Start is called before the first frame update
    void Start() {
        cigaretteLight.enabled = false;
    }

    // Update is called once per frame
    void Update() {

    }

    public void TurnCigaretteLightOn() {
        cigaretteLight.enabled = true;
    }

    public void TurnCigaretteLightOff() {
        cigaretteLight.enabled = false;
    }
}
