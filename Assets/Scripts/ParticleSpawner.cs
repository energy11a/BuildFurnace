using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject particlePrefab;
    private float nextSpawnTime = 2;
    private float spawnCooldown = 5;
    public float spawnRadius = 5f;   
    public int particlesPerSpawn = 20;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnCooldown;
            for (int i = 0; i < particlesPerSpawn; i++)
            {
                Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
                Instantiate(particlePrefab, randomPos, Quaternion.identity);
            }
        }
    }
}