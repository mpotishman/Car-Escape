using UnityEngine;

// Tracks the number of moves the player has made and updates the UI.
// Also notifies CoinManager on each move so it can update coin availability.

// Attach to collectible coins. Makes them rotate and bob up and down.
public class CoinMove : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float bobbingSpeed = 2f;
    [SerializeField] private float bobbingHeight = 0.3f;

    // create a variable of type coinmanager
    private CoinManager coinManager;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;

        // get it here
        coinManager = FindAnyObjectByType<CoinManager>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // check to see if the coin has collided with a black ncar, if it has call the other function and set the coin to false
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlackCar"))
        {
            coinManager.OnCoinCollected();
            gameObject.SetActive(false);
        }
    }
}