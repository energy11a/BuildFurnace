using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events Instance;
    public event Action OnSimulationStart;
    public event Action OnSimulationEnd;
    public bool simulating;
    public event Action<float> OnTempChange;

    void Awake() { Instance = this; }
    public void RaiseSimulationStart()
    {
        simulating = true;
        
        OnSimulationStart?.Invoke();
    }

    public void RaiseSimulationEnd()
    {
        simulating = false;
        OnSimulationEnd?.Invoke();
    }

    public void RaiseTempChange(float t) => OnTempChange?.Invoke(t);
}
