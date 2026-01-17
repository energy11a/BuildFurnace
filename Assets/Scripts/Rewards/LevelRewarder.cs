using UnityEngine;

public class LevelRewarder : MonoBehaviour
{
    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager != null)
            _gameManager.LevelWon += OnLevelWon;
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
            _gameManager.LevelWon -= OnLevelWon;
    }
    private void OnLevelWon()
    {
        if (_gameManager == null)
        {
            Debug.LogWarning("[LevelRewarder] GameManager instance is null");
            return;
        }

        if (_gameManager.level.completed) return;
        _gameManager.level.completed = true;

        int reward = Mathf.Max(0, _gameManager.level.reward);
        if (Wallet.Instance != null)
        {
            Wallet.Instance.Add(reward);
            Debug.Log($"[LevelRewarder] Awarded {reward} coins. Total: {Wallet.Instance.Coins}");
        }
    }
}
