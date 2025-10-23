using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject particlePrefab;
    private float nextSpawnTime = 2;
    private float spawnCooldown = 5;
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnCooldown;
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
        }
        
    }
}
