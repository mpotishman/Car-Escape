using UnityEngine;

// Simple traffic car used by TrafficSpawner.
// The spawner decides when a car appears; this script just moves it and turns it off when it has travelled far enough.
public class AutoMovingCar : MonoBehaviour
{
    public enum MoveDirection { Left, Right }

    // define vairables as well as an array of car renders
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 24f;
    [SerializeField] private float despawnDistance = 80f;
    [SerializeField] private Renderer[] carRenderers;
    [SerializeField] private string bodyMaterialName = "New Material 5";

    // define variables like a traffic spawner, direction of cars and positions
    private TrafficSpawner owner;
    private MoveDirection direction;
    private Vector3 startPosition;
    private Vector3 moveVector;
    private float moveSpeed;
    private bool isRunning;

    // create a function that gets a car and places in the spawner
    public void ActivateFromSpawner(Vector3 spawnPosition, MoveDirection newDirection, TrafficSpawner newOwner)
    {
        owner = newOwner;
        direction = newDirection;
        startPosition = spawnPosition;
        transform.position = spawnPosition;

        moveVector = direction == MoveDirection.Left ? Vector3.left : Vector3.right;
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        ApplyRandomCarColour();
        isRunning = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isRunning) return;

        transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);

        float distanceTravelled = Vector3.Distance(transform.position, startPosition);
        if (distanceTravelled > despawnDistance)
        {
            FinishRun();
        }
    }

    private void FinishRun()
    {
        isRunning = false;

        if (owner != null)
        {
            owner.NotifyCarFinished(this, direction);
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        isRunning = false;
    }

    private void ApplyRandomCarColour()
    {
        if (carRenderers == null || carRenderers.Length == 0)
        {
            carRenderers = GetComponentsInChildren<Renderer>();
        }

        Color carColour = GetWeightedRandomColour();

        foreach (Renderer renderer in carRenderers)
        {
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.Contains(bodyMaterialName))
                {
                    materials[i].color = carColour;
                }
            }
        }
    }

    private Color GetWeightedRandomColour()
    {
        float roll = Random.value;

        if (roll < 0.45f)
        {
            float shade = Random.Range(0.03f, 0.12f);
            return new Color(shade, shade, shade);
        }

        if (roll < 0.85f)
        {
            float shade = Random.Range(0.25f, 0.45f);
            return new Color(shade, shade, shade);
        }

        if (roll < 0.91f) return new Color(0.7f, 0.1f, 0.1f);
        if (roll < 0.95f) return new Color(0.1f, 0.25f, 0.7f);
        if (roll < 0.98f) return new Color(0.9f, 0.8f, 0.15f);
        return new Color(0.1f, 0.6f, 0.3f);
    }
}
