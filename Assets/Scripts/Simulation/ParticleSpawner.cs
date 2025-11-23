using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ParticleSpawner : MonoBehaviour
{
    public static ParticleSpawner Instance;
    [Header("Spawn Settings")]
    public GameObject particlePrefab;
    public float spawnRadius = 5f;
    public float spawnRate = 0.2f; 
    public float spawnHeight = 1f;

    private float nextSpawnTime = 0f;
    
    public float simulationDuration = 7;
    private float nextStateSwitch;
    public bool simulating = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Events.Instance.OnSimulationStart += StartSimulation;
        Events.Instance.OnSimulationEnd += EndSimulation;
    }

    private void EndSimulation()
    {
        simulating = false;
        nextStateSwitch = Time.time;
    }
    private void StartSimulation()
    {
        Debug.Log("Starting simulation");
        simulating = true;
        nextStateSwitch = Time.time + simulationDuration;
        nextSpawnTime = Time.time;
    }

    void Update()
    {
        if (!particlePrefab) return;
        if (!simulating) return;
        if (Time.time > nextStateSwitch)
        {
            Events.Instance.RaiseSimulationEnd();
            Debug.Log("Simulation end");
            return;
        }
        
        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;

            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y = Mathf.Max(randomPos.y, spawnHeight);

            Instantiate(particlePrefab, randomPos, Quaternion.identity);
        }
    }
}
