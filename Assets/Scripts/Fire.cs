using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float averageTime = 1f;
    public string particleTag = "Particle";
    private Queue<float> absorptionTimes = new Queue<float>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(particleTag)) return;
        absorptionTimes.Enqueue(Time.time);
    }
    void Update()
    {
        float currentTime = Time.time;
        if (absorptionTimes.Count > 0 && currentTime - absorptionTimes.Peek() > averageTime)
        {
            absorptionTimes.Dequeue();
        }
        float particleRate = absorptionTimes.Count / averageTime;
        Debug.Log("Particle Absorption Rate: " + particleRate + " particles per second");
    }
}
