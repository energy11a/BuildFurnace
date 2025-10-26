using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject particlePrefab;
    public float spawnRadius = 5f;
    public float spawnRate = 0.2f; 
    public float spawnHeight = 1f;

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (!particlePrefab) return;

        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;

            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y = Mathf.Max(randomPos.y, spawnHeight);

            Instantiate(particlePrefab, randomPos, Quaternion.identity);
        }
    }
}
