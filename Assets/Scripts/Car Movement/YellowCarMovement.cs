using UnityEngine;
// Attach this script to EVERY yellow car.
// All yellow cars move left/right together with A/D keys.
public class YellowCarMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 80f;
    [SerializeField] private float acceleration = 80f;
    [SerializeField] private float deceleration = 15f;

    private Rigidbody rb;
    private MoveCounter moveCounter;

    // reference to tutorial script — will be null if not in tutorial level
    private TutorialText tutorial;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionZ
                       | RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotation;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        moveCounter = FindAnyObjectByType<MoveCounter>();

        // find tutorial script if it exists in this scene
        tutorial = FindAnyObjectByType<TutorialText>();
    }

    private void FixedUpdate()
    {
        // if tutorial exists and yellow cars aren't unlocked yet, block all movement
        if (tutorial != null && !tutorial.YellowCarAllowed) return;

        float input = 0f;

        if (Input.GetKey(KeyCode.A)) input = 1f;
        else if (Input.GetKey(KeyCode.D)) input = -1f;

        if (input != 0f)
        {
            float newX = rb.linearVelocity.x + input * acceleration * Time.fixedDeltaTime;
            newX = Mathf.Clamp(newX, -maxSpeed, maxSpeed);
            rb.linearVelocity = new Vector3(newX, 0f, rb.linearVelocity.z);
        }
        else
        {
            float newX = Mathf.MoveTowards(rb.linearVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(newX, 0f, rb.linearVelocity.z);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        if (Mathf.Abs(normal.x) > 0.5f && Vector3.Dot(rb.linearVelocity, normal) < 0)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    // check to see if yellow car has hit the far wall or back wall
    private void OnCollisionEnter(Collision collision)
    {
        if (tutorial == null) return;

        if (collision.gameObject.CompareTag("TutorialYellowGoal"))
        {
            tutorial.AdvanceToStep3();
        }
    }
}