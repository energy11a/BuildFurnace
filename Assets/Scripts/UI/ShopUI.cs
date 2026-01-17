using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private string levelSelectSceneName = "LevelSelect";

    private List<ShopItemButton> itemButtons = new List<ShopItemButton>();
    [SerializeField] private ShopItemButton _itemButtonPrefab;

    private void Awake()
    {
        if (Wallet.Instance == null)
            Debug.LogError("[ShopUI] Wallet.Instance is NULL. Ensure NeededInAll (or Bootstrap) has created Wallet and it persists.");
        //if (Events.Instance == null)
        //Debug.LogError("[ShopUI] Events.Instance is NULL. Ensure NeededInAll has Events and it persists.");
    }
    private void Start()
    {
        
        Debug.Log("[ShopUI] Avaiable blocks:"  + GameManager.Instance.allBlocks.Count);
        foreach (BlockData data in GameManager.Instance.allBlocks)
        {
            Debug.Log("[ShopUI] Instantiating " + data.alias);
            ShopItemButton button = Instantiate(_itemButtonPrefab, transform);
            button.Init(data);
            itemButtons.Add(button);
        }

        RefreshCoinText();

        if (GameManager.Instance != null)
            GameManager.Instance.OnCoinsChanged += OnCoinsChanged;

        foreach (var btn in itemButtons)
        {
            btn.RefreshAffordableState(Wallet.Instance != null ? Wallet.Instance.Coins : 0);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCoinsChanged -= OnCoinsChanged;
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

        if (item.block.bought)
        {
            Debug.Log($"[ShopUI] Item '{item.block.alias}' already purchased for next level.");
            item.SetPurchasedVisual(true);
            return;
        }

        bool ok = Wallet.Instance.Spend(item.block.cost);
        if (!ok)
        {
            Debug.Log($"[ShopUI] Not enough coins to buy '{item.block.alias}'. Need {item.block.cost}, have {Wallet.Instance.Coins}.");
            item.PulseNotEnough();
            return;
        }

        item.block.bought = true;
        Debug.Log($"[ShopUI] Bought '{item.block.alias}' for {item.block.cost}. Coins now {Wallet.Instance.Coins}.");

        item.SetPurchasedVisual(true);
        foreach (var btn in itemButtons)
            btn.RefreshAffordableState(Wallet.Instance.Coins);
    }
}
