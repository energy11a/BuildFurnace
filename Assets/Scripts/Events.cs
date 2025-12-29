using System;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    [HideInInspector]public static Events Instance;
    [HideInInspector]public event Action OnSimulationStart;
    [HideInInspector]public event Action OnSimulationEnd;
    [HideInInspector]public bool simulating;
    [HideInInspector]public event Action<float> OnOxygenChange;
    [HideInInspector]public event Action<float> OnTempChange;
    [HideInInspector]public event Action LevelWon;
    [HideInInspector]public event Action<int> OnCoinsChanged;

    public List<LevelData> levels;
    [HideInInspector]public int currentLevel;
    //Audio
    [SerializeField] private AudioClip startLevelSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
        if (t >= levels[currentLevel].winTemperature)
        {
            RaiseLevelWon();
        }
        OnTempChange?.Invoke(t);
    }

    public void RaiseOxygenLevelChanged(float t)
    {
        OnOxygenChange.Invoke(t);
    }

    public void RaiseLevelWon()
    {
        LevelWon?.Invoke();
    }
    private void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void RaiseCoinsChanged(int totalCoins) => OnCoinsChanged?.Invoke(totalCoins);

    public void LoadLevel(int index)
    {
        currentLevel = index;
        LevelData level = levels[currentLevel];
        
        if (startLevelSound) audioSource.PlayOneShot(startLevelSound);
        foreach (CubeEntry cubeEntry in level.buildingBlocks){
            
        }

        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync(level.SceneName, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("NeededInAll", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }
}
