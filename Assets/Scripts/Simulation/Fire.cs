using System;
using System.Collections.Generic;
using Simulation;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Fire : MonoBehaviour
{
    public static Fire Instance;
    private bool simulating = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public HeatParticle heatParticlePrefab;
    public string particleTag = "Particle";
    public GameObject winPanel;
    public float winTemperature = 100f;

    public float heatAreaVolume;
    public HashSet<HeatParticle> heatParticles = new HashSet<HeatParticle>();

    public float oxygenConcentration = 100;
    public float addPerOxygenParticle = 10;
    public float loseOxygenPerSecond = 10;

    private float nextHeatSpawnTime;
    public float heatSpawnCooldown = 0.2f;

    [Tooltip("Temperature used for areas not covered by any heat particles")]
    public float roomTemperature = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(particleTag)) return;
        oxygenConcentration += addPerOxygenParticle;
    }

    private void Update()
    {
        oxygenConcentration -= loseOxygenPerSecond * Time.deltaTime;

        if (oxygenConcentration <= 0f)
        {
            Events.Instance.RaiseSimulationEnd();
            Debug.Log("Ran out of oxygen");
        }

        if (nextHeatSpawnTime <= Time.time)
        {
            nextHeatSpawnTime = Time.time + heatSpawnCooldown;
            HeatParticle heatParticle = Instantiate(heatParticlePrefab, transform.position + Vector3.up * 0.75f, Quaternion.identity);
            heatParticle.temperature = oxygenConcentration;
        }

        float temperature = CalculateTemperature();
        Events.Instance.RaiseTempChange(temperature);
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
            if (!particle) 
                continue;

            // Assume another script sets this on HeatParticle
            // e.g. public float area;
            float area = Mathf.Max(0f, particle.area);

            totalCoveredArea += area;
            totalHeat += area * particle.temperature;
        }

        // Clamp covered area so it never exceeds total area
        totalCoveredArea = Mathf.Min(totalCoveredArea, heatAreaVolume);

        // Uncovered area gets room temperature
        float uncoveredArea = heatAreaVolume - totalCoveredArea;
        if (uncoveredArea > 0f)
        {
            totalHeat += uncoveredArea * roomTemperature;
        }

        // Final average temperature over the whole area
        return  totalHeat / heatAreaVolume;
    }
}
