using UnityEngine;

public class BlackCarMovement : MonoBehaviour
{
    // all vairbales defined here are ones that can be changed in inspector
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 15f;

    // define rigidbody for the car so physics works on it
    private Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // grab the object which has the rigidbody (the black car)
        rb = GetComponent<Rigidbody>();

        // now stop it from being able to move on the x axis, y axis, and spin
        rb.constraints = RigidbodyConstraints.FreezePositionX
                       | RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotation;
    }

    // FixedUpdate is called once per physics check of a rigid body
    void FixedUpdate()
    {
        // define a input float of a cars positional value, positive = forward negative = backwards
        float input = 0f;

        // check if user pressed space or x button
        if (Input.GetKey(KeyCode.Space)) input = 1f;
        else if (Input.GetKey(KeyCode.X)) input = -1f;

        // if user holds key accelerate in that direction
        if (input != 0f)
        {
            // take the current z velocity and add it to this frame
            // so create a new z velocity by taking the current one, adding the input and multiplying by time of physics step to add acceleration uniformly
            float newZ = rb.linearVelocity.z + input * acceleration * Time.fixedDeltaTime;

            // add a clamp to ensure values dont go too high or low (can only be in between these values)
            newZ = Mathf.Clamp(newZ, -maxSpeed, maxSpeed);

            // assign the new velocity 
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, newZ);
        }
        else
        {
            // no key held, smoothly slow z velocity down to 0
            // Mathf.MoveTowards(current, target, maxStep) moves current toward target
            // by at most maxStep, without overshooting.
            float newZ = Mathf.MoveTowards(rb.linearVelocity.z, 0f, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, newZ);

        }

    }
}
