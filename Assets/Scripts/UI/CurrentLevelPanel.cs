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

    public void ChangeDesc(int level, int goal, int reward, string extra = "") 
    {
        currentLevel = level;
        goalTemp = goal;
        rewardAmount = reward;
        extraInfo = extra;

        UpdateTexts();
    }
    
    void UpdateTexts() 
    {
        
    }

}
