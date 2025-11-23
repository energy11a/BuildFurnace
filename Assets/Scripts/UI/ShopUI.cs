using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private string levelSelectSceneName = "LevelSelect";

    private ShopItemButton[] itemButtons;

    private void Awake()
    {
        if (Wallet.Instance == null)
            Debug.LogError("[ShopUI] Wallet.Instance is NULL. Ensure NeededInAll (or Bootstrap) has created Wallet and it persists.");
        //if (Events.Instance == null)
            //Debug.LogError("[ShopUI] Events.Instance is NULL. Ensure NeededInAll has Events and it persists.");
    }
    private void Start()
    {
        itemButtons = GetComponentsInChildren<ShopItemButton>(true);

        RefreshCoinText();

        if (Events.Instance != null)
            Events.Instance.OnCoinsChanged += OnCoinsChanged;

        foreach (var btn in itemButtons)
        {
            btn.RefreshAffordableState(Wallet.Instance != null ? Wallet.Instance.Coins : 0);
        }
    }

    private void OnDestroy()
    {
        if (Events.Instance != null)
            Events.Instance.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int total)
    {
        RefreshCoinText();
        foreach (var btn in itemButtons)
            btn.RefreshAffordableState(total);
    }

    private void RefreshCoinText()
    {
        if (coinText != null)
            coinText.text = Wallet.Instance != null ? Wallet.Instance.Coins.ToString() : "0";
    }

    public void GoBackToLevelSelect()
    {
        SceneManager.LoadScene(levelSelectSceneName, LoadSceneMode.Single);
    }
    public void TryPurchase(ShopItemButton item)
    {
        if (Wallet.Instance == null)
        {
            Debug.LogError("[ShopUI] Wallet.Instance missing; cannot purchase.");
            return;
        }

        if (TempInventory.HasItem(item.ItemId))
        {
            Debug.Log($"[ShopUI] Item '{item.ItemId}' already purchased for next level.");
            item.SetPurchasedVisual(true);
            return;
        }

        bool ok = Wallet.Instance.Spend(item.Cost);
        if (!ok)
        {
            Debug.Log($"[ShopUI] Not enough coins to buy '{item.ItemId}'. Need {item.Cost}, have {Wallet.Instance.Coins}.");
            item.PulseNotEnough();
            return;
        }
        TempInventory.AddItem(item.ItemId);
        Debug.Log($"[ShopUI] Bought '{item.ItemId}' for {item.Cost}. Coins now {Wallet.Instance.Coins}.");

        item.SetPurchasedVisual(true);
        foreach (var btn in itemButtons)
            btn.RefreshAffordableState(Wallet.Instance.Coins);
    }
}
