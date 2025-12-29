using UnityEngine;

public class LevelRewarder : MonoBehaviour
{
    public CurrentLevelPanel levelPanel;

    private void OnEnable()
    {
        if (Events.Instance != null)
            Events.Instance.LevelWon += OnLevelWon;
    }
    private void OnDisable()
    {
        if (Events.Instance != null)
            Events.Instance.LevelWon -= OnLevelWon;
    }
    private void OnLevelWon()
    {
        if (levelPanel == null)
        {
            Debug.LogWarning("[LevelRewarder] No CurrentLevelPanel assigned; reward is 0.");
            return;
        }

        int reward = Mathf.Max(0, Events.Instance.level.reward);
        if (Wallet.Instance != null)
        {
            Wallet.Instance.Add(reward);
            Debug.Log($"[LevelRewarder] Awarded {reward} coins. Total: {Wallet.Instance.Coins}");
        }
    }
}
