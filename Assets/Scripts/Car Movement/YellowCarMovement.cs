// "using" imports a namespace — a library of pre-built code.
// UnityEngine gives us access to MonoBehaviour, Rigidbody, Input, Vector3, etc.
using UnityEngine;

// Attach this script to EVERY yellow car.
// All yellow cars move left/right together with A/D keys.

// "public" means other scripts can see this class.
// "class" defines a new type — this is the blueprint for the script component.
// ": MonoBehaviour" means this class inherits from MonoBehaviour, which is Unity's
// base class for all scripts. It gives us Start(), Update(), etc. for free.
public class YellowCarMovement : MonoBehaviour
{
    // [SerializeField] makes a private variable show up in the Unity Inspector,
    // so you can tweak the value without changing the code.
    // "private" means only this script can read/write this variable.
    // "float" is a decimal number (e.g. 8.0). The "f" suffix on the value marks it as a float.
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 15f;

    // Rigidbody is Unity's physics component. We store a reference to it here
    // so we don't have to look it up every frame (that would be slow).
    private Rigidbody rb;
    private MoveCounter moveCounter;

    // Start() is called once by Unity when this object first appears in the scene.
    // "private void" means it returns nothing and only this class calls it.
    private void Start()
    {
        // GetComponent<T>() searches this GameObject for a component of type T.
        // We grab the Rigidbody and store it in our "rb" variable for later use.
        rb = GetComponent<Rigidbody>();

        // Find the MoveCounter in the scene so we can tell it when a move happens
        moveCounter = FindObjectOfType<MoveCounter>();

        // constraints locks certain axes so physics can't move or rotate the car
        // on those axes. The "|" symbol combines multiple flags together (bitwise OR).
        rb.constraints = RigidbodyConstraints.FreezePositionZ  // can't slide forward/back
                       | RigidbodyConstraints.FreezePositionY  // can't fly up or fall down
                       | RigidbodyConstraints.FreezeRotation;  // can't tip or spin

        // Continuous collision detection checks for collisions between physics steps,
        // preventing fast-moving objects from tunneling through thin colliders.
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    // FixedUpdate() is called by Unity on a fixed physics timestep (default 50x per second).
    // Always use FixedUpdate (not Update) when moving Rigidbodies, so physics stays stable.
    private void FixedUpdate()
    {
        // Declare a local float to store the player's directional input.
        // We start at 0 (no input), then check for key presses below.
        float input = 0f;

        // Input.GetKey() returns true every frame the key is held down.
        // We map A to +1 and D to -1 (swap these if the direction feels backwards).
        if (Input.GetKey(KeyCode.A)) input = 1f;
        else if (Input.GetKey(KeyCode.D)) input = -1f;

        // If a key is held, accelerate in that direction.
        if (input != 0f)
        {
            // Take the current X velocity and add to it this frame.
            // Time.fixedDeltaTime is the length of one physics step in seconds —
            // multiplying by it makes the result frame-rate independent.
            float newX = rb.linearVelocity.x + input * acceleration * Time.fixedDeltaTime;

            // Mathf.Clamp keeps newX between -maxSpeed and +maxSpeed so we don't
            // accelerate forever.
            newX = Mathf.Clamp(newX, -maxSpeed, maxSpeed);

            // Assign the new velocity. Vector3 is a struct with x, y, z components.
            // We keep z and y unchanged and only update x.
            rb.linearVelocity = new Vector3(newX, 0f, rb.linearVelocity.z);
        }
        else
        {
            // No key held — smoothly slow the X velocity down to 0.
            // Mathf.MoveTowards(current, target, maxStep) moves current toward target
            // by at most maxStep, without overshooting.
            float newX = Mathf.MoveTowards(rb.linearVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(newX, 0f, rb.linearVelocity.z);
        }
    }

    // OnCollisionStay() is called by Unity every physics step while two colliders
    // are touching. "Collision collision" contains info about what we hit.
    private void OnCollisionStay(Collision collision)
    {
        // contacts[0] is the first contact point. .normal is the direction the
        // surface is facing — it points away from whatever we collided with.
        Vector3 normal = collision.contacts[0].normal;

        // If the collision normal is mostly along the X axis, we've hit a wall
        // on our movement axis. Zero out X velocity so held input can't push
        // the car through the obstacle.
        // Mathf.Abs() gives the absolute (positive) value so we catch both directions.
        // Only cancel velocity if we're moving INTO the wall, not away from it.
        // Dot product < 0 means velocity and normal point opposite directions (pressing into wall).
        if (Mathf.Abs(normal.x) > 0.5f && Vector3.Dot(rb.linearVelocity, normal) < 0)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, rb.linearVelocity.z);
    }
}
