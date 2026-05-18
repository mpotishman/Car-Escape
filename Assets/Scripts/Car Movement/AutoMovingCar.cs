using UnityEngine;

// Replaces both CarBelowMove.cs and Lightbluemove.cs.
// Cars that move automatically and loop back when they go off screen.
// Set direction and boundary in the Inspector per car.
public class AutoMovingCar : MonoBehaviour
{
    public enum MoveDirection { Up, Down, Left, Right }

    [SerializeField] private MoveDirection direction = MoveDirection.Up;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetBoundary = 53f; // distance from start before looping

    private Vector3 startPosition;
    private Vector3 moveVector;

    private void Start()
    {
        startPosition = transform.position;

        moveVector = direction switch
        {
            MoveDirection.Up    => Vector3.forward,
            MoveDirection.Down  => Vector3.back,
            MoveDirection.Left  => Vector3.left,
            MoveDirection.Right => Vector3.right,
            _                   => Vector3.forward
        };
    }

    private void Update()
    {
        transform.Translate(moveVector * speed * Time.deltaTime, Space.World);

        float distanceTravelled = Vector3.Distance(transform.position, startPosition);
        if (distanceTravelled > resetBoundary)
            transform.position = startPosition;
    }
}
