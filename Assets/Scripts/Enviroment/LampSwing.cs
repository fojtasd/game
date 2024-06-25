using UnityEngine;

public class LampSwing : MonoBehaviour {
    Rigidbody2D rigidBody;

    [Range(0f, 3f)]
    public float torque = 0.31f;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        SwingLamp();
    }

    public void SwingLamp() {
        rigidBody.AddTorque(torque, ForceMode2D.Impulse);
    }
}
