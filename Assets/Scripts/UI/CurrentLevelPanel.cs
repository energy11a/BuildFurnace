using TMPro;
using UnityEngine;

public class CurrentLevelPanel : MonoBehaviour
{
    public int currentLevel;
    public int goalTemp;
    public int rewardAmount;
    public string extraInfo;

    public GameObject levelText;
    public GameObject goalText;
    public GameObject rewardText;
    public GameObject extraText;

    TMP_Text extraTmp;

    void Awake()
    {
        if (extraText) extraTmp = extraText.GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        Events.Instance.OnTempChange += OnTempChange;
    }

    void OnDisable()
    {
        if (Events.Instance != null) Events.Instance.OnTempChange -= OnTempChange;
    }

    public void ChangeDesc(int level, int goal, int reward, string extra = "")
    {
        currentLevel = level;
        goalTemp = goal;
        rewardAmount = reward;
        extraInfo = extra;
    }

    void OnTempChange(float t)
    {
        if (extraTmp) extraTmp.text = $"Current temp: {t:F0} °C";
    }
}
