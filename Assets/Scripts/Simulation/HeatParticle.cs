using UnityEngine;
using Random = UnityEngine.Random;

namespace Simulation
{
    public class HeatParticle : MonoBehaviour
    {
        public float temperature = 1f;
        public float area;

        [Tooltip("How much heat is lost per second")]
        public float heatDissipationRate = 45.0f;

        [Tooltip("Initial movement direction; will be changed randomly over time")]
        public Vector3 moveDirection = Vector3.zero;

        [Tooltip("Strength of the movement force")]
        public float moveForce = 1f;

        [Tooltip("How often (in seconds) to pick a new random direction")]
        public float directionChangeInterval = 1f;

        private Rigidbody rb;
        private float directionTimer;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            directionTimer = directionChangeInterval;

            SphereCollider col = GetComponent<SphereCollider>();
            float radius = col.radius * transform.lossyScale.x;
            area = (float)(0.5236 * Mathf.Pow(2 * radius, 3)); //Approximation
        }

        private void Update()
        {
            temperature -= heatDissipationRate * Time.deltaTime;

            if (temperature <= 0f)
            {
                Fire.Instance.heatParticles.Remove(this);
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(Fire.Instance.transform.position, transform.position) > 10) Destroy(gameObject);
            if (!rb) return;
            rb.AddForce(moveDirection * moveForce, ForceMode.Acceleration);
            directionTimer -= Time.fixedDeltaTime;
            if (directionTimer <= 0f)
            {
                directionTimer = directionChangeInterval;
                moveDirection = Random.onUnitSphere;
            }
        }
    }
}