using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    public TMP_Text coinText;

    private void OnEnable()
    {
        if (coinText != null)
            coinText.text = Wallet.Instance ? Wallet.Instance.Coins.ToString() : "0";
            coinText.text = "Total: " + coinText.text;

        if (Events.Instance != null)
            Events.Instance.OnCoinsChanged += OnCoinsChanged;
    }

    private void OnDisable()
    {
        if (Events.Instance != null)
            Events.Instance.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int total)
    {
        if (coinText != null)
            coinText.text = total.ToString();
    }
}