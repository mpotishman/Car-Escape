using UnityEngine;

// Attach to yellow cars (move up/down only).
// Set keyPositive and keyNegative in the Inspector per car.
[RequireComponent(typeof(Rigidbody))]
public class CarUpAndDown : MonoBehaviour
{
    [SerializeField] private KeyCode keyPositive;
    [SerializeField] private KeyCode keyNegative;
    [SerializeField] private float speed = 5f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(keyPositive))
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, speed, rb.linearVelocity.z);
        else if (Input.GetKey(keyNegative))
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -speed, rb.linearVelocity.z);
        else
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }
}
