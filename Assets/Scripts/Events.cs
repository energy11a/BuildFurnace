using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events Instance;
    public event Action OnSimulationStart;
    public event Action OnSimulationEnd;
    public event Action<float> OnTempChange;
    public event Action LevelWon;
    public event Action<int> OnCoinsChanged;

    void Awake() {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void RaiseSimulationStart() => OnSimulationStart?.Invoke();
    public void RaiseSimulationEnd()   => OnSimulationEnd?.Invoke();
    public void RaiseTempChange(float t) => OnTempChange?.Invoke(t);
    public void RaiseLevelWon() => LevelWon?.Invoke();
    public void RaiseCoinsChanged(int totalCoins) => OnCoinsChanged?.Invoke(totalCoins);
}
