using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Button backButton;

    void Start()
    {
        if (coinText != null)
            coinText.text = Wallet.Instance != null ? Wallet.Instance.Coins.ToString() : "0";

        if (Events.Instance != null)
            Events.Instance.OnCoinsChanged += UpdateCoinDisplay;

        if (backButton != null)
            backButton.onClick.AddListener(() => SceneManager.LoadScene("LevelSelect"));
    }

    void OnDestroy()
    {
        if (Events.Instance != null)
            Events.Instance.OnCoinsChanged -= UpdateCoinDisplay;
    }

    void UpdateCoinDisplay(int coins)
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }

    public void BuyItem(int cost)
    {
        if (Wallet.Instance != null && Wallet.Instance.Spend(cost))
        {
            Debug.Log($"Bought item for {cost} coins!");
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }
}
