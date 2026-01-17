using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    public TMP_Text coinText;

    private void OnEnable()
    {
        if (coinText != null)
            coinText.text = Wallet.Instance ? Wallet.Instance.Coins.ToString() : "0";
            coinText.text = "Coins: " + coinText.text;

        if (GameManager.Instance != null)
            GameManager.Instance.OnCoinsChanged += OnCoinsChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int total)
    {
        if (coinText != null)
            coinText.text = "Coins: " + total.ToString();
    }
}