// "using" imports a namespace — a library of pre-built code.
// UnityEngine gives us access to MonoBehaviour, Rigidbody, Input, Vector3, etc.
using UnityEngine;

// Attach this script to EVERY red car.
// All red cars move forward/back together with Left/Right arrow keys.

// "public" means other scripts can see this class.
// "class" defines a new type — this is the blueprint for the script component.
// ": MonoBehaviour" means this class inherits from MonoBehaviour, which is Unity's
// base class for all scripts. It gives us Start(), Update(), etc. for free.
public class RedCarMovement : MonoBehaviour
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

        // constraints locks certain axes so physics can't move or rotate the car
        // on those axes. The "|" symbol combines multiple flags together (bitwise OR).
        rb.constraints = RigidbodyConstraints.FreezePositionX  // can't slide left/right
                       | RigidbodyConstraints.FreezePositionY  // can't fly up or fall down
                       | RigidbodyConstraints.FreezeRotation;  // can't tip or spin


        moveCounter = FindObjectOfType<MoveCounter>();
    }

    // FixedUpdate() is called by Unity on a fixed physics timestep (default 50x per second).
    // Always use FixedUpdate (not Update) when moving Rigidbodies, so physics stays stable.
    private void FixedUpdate()
    {
        // Declare a local float to store the player's directional input.
        // We start at 0 (no input), then check for key presses below.
        float input = 0f;

        // Input.GetKey() returns true every frame the key is held down.
        // We map Right arrow to +1 (forward) and Left arrow to -1 (backward).
        if (Input.GetKey(KeyCode.RightArrow)) input = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) input = -1f;

        // If a key is held, accelerate in that direction.
        if (input != 0f)
        {
            // Take the current Z velocity and add to it this frame.
            // Time.fixedDeltaTime is the length of one physics step in seconds —
            // multiplying by it makes the result frame-rate independent.
            float newZ = rb.linearVelocity.z + input * acceleration * Time.fixedDeltaTime;

            // Mathf.Clamp keeps newZ between -maxSpeed and +maxSpeed so we don't
            // accelerate forever.
            newZ = Mathf.Clamp(newZ, -maxSpeed, maxSpeed);

            // Assign the new velocity. Vector3 is a struct with x, y, z components.
            // We keep x and y unchanged and only update z.
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, newZ);
        }
        else
        {
            // No key held — smoothly slow the Z velocity down to 0.
            // Mathf.MoveTowards(current, target, maxStep) moves current toward target
            // by at most maxStep, without overshooting.
            float newZ = Mathf.MoveTowards(rb.linearVelocity.z, 0f, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, newZ);
        }
    }
}
