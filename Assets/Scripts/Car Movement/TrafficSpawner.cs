using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [Header("Car Pools")]
    [SerializeField] private AutoMovingCar[] leftCars;
    [SerializeField] private AutoMovingCar[] rightCars;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] leftSpawnPoints;
    [SerializeField] private Transform[] rightSpawnPoints;

    [Header("Traffic Timing")]
    [SerializeField] private float baseSpawnInterval = 1.4f;
    [SerializeField] private float randomExtraDelay = 0.8f;

    [Header("Traffic Limits")]
    [SerializeField] private int maxActiveLeftCars = 2;
    [SerializeField] private int maxActiveRightCars = 2;

    private readonly List<AutoMovingCar> activeLeftCars = new();
    private readonly List<AutoMovingCar> activeRightCars = new();

    private void Awake()
    {
        DeactivatePool(leftCars);
        DeactivatePool(rightCars);
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop(AutoMovingCar.MoveDirection.Left));
        StartCoroutine(SpawnLoop(AutoMovingCar.MoveDirection.Right));
    }

    private IEnumerator SpawnLoop(AutoMovingCar.MoveDirection direction)
    {
        yield return new WaitForSeconds(Random.Range(0f, baseSpawnInterval));

        while (true)
        {
            TrySpawn(direction);

            float delay = baseSpawnInterval + Random.Range(0f, randomExtraDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private void TrySpawn(AutoMovingCar.MoveDirection direction)
    {
        List<AutoMovingCar> activeCars = direction == AutoMovingCar.MoveDirection.Left ? activeLeftCars : activeRightCars;
        int activeLimit = direction == AutoMovingCar.MoveDirection.Left ? maxActiveLeftCars : maxActiveRightCars;

        if (activeCars.Count >= activeLimit)
        {
            return;
        }

        AutoMovingCar[] carPool = direction == AutoMovingCar.MoveDirection.Left ? leftCars : rightCars;
        Transform[] spawnPoints = direction == AutoMovingCar.MoveDirection.Left ? leftSpawnPoints : rightSpawnPoints;

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return;
        }

        AutoMovingCar carToSpawn = GetInactiveCar(carPool);
        if (carToSpawn == null)
        {
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        activeCars.Add(carToSpawn);
        carToSpawn.ActivateFromSpawner(spawnPoint.position, direction, this);
    }

    private AutoMovingCar GetInactiveCar(AutoMovingCar[] carPool)
    {
        if (carPool == null || carPool.Length == 0)
        {
            return null;
        }

        List<AutoMovingCar> availableCars = new();

        for (int i = 0; i < carPool.Length; i++)
        {
            if (carPool[i] != null && !carPool[i].gameObject.activeSelf)
            {
                availableCars.Add(carPool[i]);
            }
        }

        if (availableCars.Count == 0)
        {
            return null;
        }

        return availableCars[Random.Range(0, availableCars.Count)];
    }

    public void NotifyCarFinished(AutoMovingCar car, AutoMovingCar.MoveDirection direction)
    {
        List<AutoMovingCar> activeCars = direction == AutoMovingCar.MoveDirection.Left ? activeLeftCars : activeRightCars;
        activeCars.Remove(car);
    }

    private void DeactivatePool(AutoMovingCar[] carPool)
    {
        if (carPool == null) return;

        for (int i = 0; i < carPool.Length; i++)
        {
            if (carPool[i] != null)
            {
                carPool[i].gameObject.SetActive(false);
            }
        }
    }
}
