using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Fire : MonoBehaviour
{
    public string particleTag = "Particle";
    public float averagingWindow = 1f;
    private readonly Queue<float> absorptionTimes = new Queue<float>();
    public float particlesPerSecond { get; private set; }
    public float tempMax = 500f;
    public float k = 0.5f;
    public float ppsMax = 5000f;
    public float smoothingAlpha = 0.2f;
    public float coolingPerSecond = 30f;
    public float Temperature { get; private set; }
    public GameObject winPanel;
    public float winTemperature = 100f;
    public bool disablePanelOnStart = true;
    private bool hasWon;

    float tick;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(particleTag)) absorptionTimes.Enqueue(Time.time);
    }

    void Update()
    {
        float now = Time.time;
        while (absorptionTimes.Count > 0 && now - absorptionTimes.Peek() > averagingWindow) absorptionTimes.Dequeue();
        particlesPerSecond = absorptionTimes.Count / averagingWindow;
        float instantTemp = TemperatureLog(particlesPerSecond);
        Temperature = Mathf.Lerp(Temperature, instantTemp, smoothingAlpha);
        if (particlesPerSecond <= 0.01f) Temperature = Mathf.Max(0f, Temperature - coolingPerSecond * Time.deltaTime);

        tick += Time.deltaTime;
        if (tick >= 0.2f)
        {
            tick = 0f;
            Events.Instance?.RaiseTempChange(Temperature);
        }

        if (!hasWon && Temperature >= winTemperature)
        {
            hasWon = true;
            Events.Instance?.RaiseLevelWon();

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
        }
    }

    float TemperatureLog(float pps)
    {
        float num = Mathf.Log(1f + k * pps);
        float den = Mathf.Log(1f + k * ppsMax);
        return tempMax * (den > 0f ? num / den : 0f);
    }
}
