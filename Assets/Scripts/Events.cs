using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events Instance;
    public event Action OnSimulationStart;
    public event Action OnSimulationEnd;
    public bool simulating;
    public event Action<float> OnTempChange;
    public float winTemperature;
    public event Action LevelWon;
    public event Action<int> OnCoinsChanged;

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

    public void RaiseTempChange(float t)
    {
        if (t >= winTemperature)
        {
            RaiseLevelWon();
        }
        OnTempChange?.Invoke(t);
    }

    public void RaiseLevelWon()
    {
        LevelWon?.Invoke();
    }

    public void RaiseCoinsChanged(int totalCoins) => OnCoinsChanged?.Invoke(totalCoins);
}
