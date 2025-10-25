using UnityEngine;

public class CurrentLevelPanel : MonoBehaviour
{
    public int currentLevel;
    public int goalTemp;
    public int rewardAmount;
    public string extraInfo;


    public void ChangeDesc(int level, int goal, int reward, string extra = "") 
    {
        currentLevel = level;
        goalTemp = goal;
        rewardAmount = reward;
        extraInfo = extra;
    }



}
