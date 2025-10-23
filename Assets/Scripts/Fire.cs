using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class Fire : MonoBehaviour
{
    public string particleTag = "Particle";
    public float averagingWindow = 1f; // seconds

    private Queue<float> absorptionTimes = new Queue<float>();
    public float particlesPerSecond { get; private set; }

    public float tempMax = 500f;   // maximum temperature
    public float k = 0.5f;          // curvature for log mapping
    public float ppsMax = 5000f;      // expected upper bound of particles/sec

    public float smoothingAlpha = 0.2f; // smoothing factor per frame
    public float coolingPerSecond = 30f;

    public float Temperature { get; private set; } // smoothed temperature

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(particleTag)) return;

        // Record absorption time
        absorptionTimes.Enqueue(Time.time);

        // Destroy or recycle the particle here if needed
        // Destroy(other.gameObject);
    }

    void Update()
    {
        float now = Time.time;

        // Remove old timestamps
        while (absorptionTimes.Count > 0 && now - absorptionTimes.Peek() > averagingWindow)
        {
            absorptionTimes.Dequeue();
        }

        // Calculate rolling rate
        particlesPerSecond = absorptionTimes.Count / averagingWindow;

        // Map to an "instantaneous" temperature
        float instantTemp = TemperatureLog(particlesPerSecond);

        // Smooth toward that value
        Temperature = Mathf.Lerp(Temperature, instantTemp, smoothingAlpha);

        // Passive cooling if no intake
        if (particlesPerSecond <= 0.01f)
        {
            Temperature = Mathf.Max(0f, Temperature - coolingPerSecond * Time.deltaTime);
        }

        // Debug
        Debug.Log($"PPS={particlesPerSecond:F2}, Temp={Temperature:F1}");
    }

    // Logarithmic mapping with saturation
    private float TemperatureLog(float pps)
    {
        float num = Mathf.Log(1f + k * pps);
        float den = Mathf.Log(1f + k * ppsMax);
        return tempMax * (den > 0f ? num / den : 0f);
    }
}
