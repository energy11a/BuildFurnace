using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Simulation
{
    [RequireComponent(typeof(SphereCollider))]
    public class Fire : MonoBehaviour
    {
        public static Fire Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            Events.Instance.OnSimulationStart += OnStart;
        }

        private void OnStart()
        {
            oxygenConcentration = startingOxygen;
        }

        public HeatParticle heatParticlePrefab;
        public string particleTag = "Particle";

        public float heatAreaVolume;
        public HashSet<HeatParticle> heatParticles = new HashSet<HeatParticle>();

        public float startingOxygen = 100;
        private float oxygenConcentration;
        public float addPerOxygenParticle = 30;
        public float loseOxygenPerSecond = 10;
        public float maxOxygen = 300;

        private float nextHeatSpawnTime;
        public float heatSpawnCooldown = 0.2f;

        [Tooltip("Temperature used for areas not covered by any heat particles")]
        public float roomTemperature = 20f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(particleTag)) return;
            oxygenConcentration += addPerOxygenParticle;
            oxygenConcentration = Mathf.Min(oxygenConcentration, maxOxygen);
        }

        private void Update()
        {
            if (!Events.Instance.simulating) return;

            float temperature = CalculateTemperature();
            Events.Instance.RaiseTempChange(temperature);

            if (oxygenConcentration <= 0f)
            {
                Events.Instance.RaiseSimulationEnd();
                return;
            }

            oxygenConcentration -= loseOxygenPerSecond * Time.deltaTime;

            if (nextHeatSpawnTime <= Time.time)
            {
                nextHeatSpawnTime = Time.time + heatSpawnCooldown;
                HeatParticle heatParticle = Instantiate(heatParticlePrefab, transform.position + Vector3.up * 0.75f,
                    Quaternion.identity);
                heatParticles.Add(heatParticle);
                heatParticle.temperature = 2 * oxygenConcentration;
            }

            Events.Instance.RaiseOxygenLevelChanged((oxygenConcentration / maxOxygen) * 100);
    }

        private float CalculateTemperature()
        {
            if (heatAreaVolume <= 0f)
            {
                return roomTemperature;
            }

            float totalHeat = 0f;
            float totalCoveredArea = 0f;

            foreach (var particle in heatParticles)
            {
                if (!particle) continue;
                float area = Mathf.Max(0f, particle.area);

                totalCoveredArea += area;
                totalHeat += area * particle.temperature;
            }

            float uncoveredArea = heatAreaVolume - totalCoveredArea;
            if (uncoveredArea > 0f)
            {
                totalHeat += uncoveredArea * roomTemperature;
            }

            return totalHeat / heatAreaVolume;
        }
    }
}