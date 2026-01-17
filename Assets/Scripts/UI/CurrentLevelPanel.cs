using System;
using Data;
using TMPro;
using UnityEngine;

public class CurrentLevelPanel : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text tempText;
    public TMP_Text oxygenText;

    TMP_Text extraTmp;

    void Awake()
    {
        if (tempText) extraTmp = tempText.GetComponent<TMP_Text>();
    }
    void Start(){
        LevelData level = GameManager.Instance.level;
        levelText.text = level.LevelName;
        OnTempChange(0);
        OnOxygenChange(0);
    }

    private void OnOxygenChange(float percentage)
    {
        oxygenText.text = "O2: " + percentage.ToString("F2") + " %";
    }
    void OnEnable()
    {
        GameManager.Instance.OnTempChange += OnTempChange;
        GameManager.Instance.OnOxygenChange += OnOxygenChange;
    }

    void OnDisable()
    {
        GameManager.Instance.OnTempChange -= OnTempChange;
        GameManager.Instance.OnOxygenChange -= OnOxygenChange;
    }

    void OnTempChange(float t)
    {
        if (extraTmp) extraTmp.text = $"{t:F0} / {GameManager.Instance.level.winTemperature} °C";
    }
}
