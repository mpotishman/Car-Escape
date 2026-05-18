using UnityEngine;

// Attach this script to EVERY red car.
// All red cars move forward/back together with Left/Right arrow keys.
public class RedCarMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 15f;

    private Rigidbody rb;
    private MoveCounter moveCounter;

    // reference to tutorial script — will be null if not in tutorial level
    private TutorialText tutorial;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionX
                       | RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotation;

        moveCounter = FindAnyObjectByType<MoveCounter>();
        tutorial = FindAnyObjectByType<TutorialText>();
    }

    private void FixedUpdate()
    {
        // if tutorial exists and red cars aren't unlocked yet, block all movement
        if (tutorial != null && !tutorial.RedCarAllowed) return;

        float input = 0f;

    
        if (Input.GetKey(KeyCode.RightArrow)) input = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) input = -1f;

        if (input != 0f)
        {
            float newZ = rb.linearVelocity.z + input * acceleration * Time.fixedDeltaTime;
            newZ = Mathf.Clamp(newZ, -maxSpeed, maxSpeed);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, newZ);
        }
        else
        {
            float newZ = Mathf.MoveTowards(rb.linearVelocity.z, 0f, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, newZ);
        }
    }

    // check to see if red car has hit the far wall
    private void OnCollisionEnter(Collision collision)
    {
        if (tutorial == null) return;

        if (collision.gameObject.CompareTag("TutorialGoal"))
        {
            tutorial.AdvanceToStep2();
        }
    }

}
