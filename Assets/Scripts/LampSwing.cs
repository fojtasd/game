using UnityEngine;

public class LampSwing : MonoBehaviour {
    private Rigidbody2D rb;

    [Range(0f, 3f)]
    public float torque = 0.31f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        // Volitelné: Spustit houpání na začátku
        SwingLamp();
    }

    // Function to apply an initial force to the lamp to make it swing
    public void SwingLamp() {
        // Apply a small torque to initiate the swing
        rb.AddTorque(torque, ForceMode2D.Impulse);
    }
}
