using UnityEngine;

public class LevelRewarder : MonoBehaviour
{
    private Events events;

    private void OnEnable()
    {
        events = Events.Instance;
        if (events != null)
            events.LevelWon += OnLevelWon;
    }
    private void OnDisable()
    {
        if (Events.Instance != null)
            events.LevelWon -= OnLevelWon;
    }
    private void OnLevelWon()
    {
        if (events == null)
        {
            Debug.LogWarning("[LevelRewarder] Events instance is null");
            return;
        }

        int reward = Mathf.Max(0, events.level.reward);
        if (Wallet.Instance != null)
        {
            Wallet.Instance.Add(reward);
            Debug.Log($"[LevelRewarder] Awarded {reward} coins. Total: {Wallet.Instance.Coins}");
        }
    }
}
