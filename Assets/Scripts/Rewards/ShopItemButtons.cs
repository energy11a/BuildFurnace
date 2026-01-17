
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private GameObject purchasedBadge;

    private ShopUI shop;

    public BlockData block;

    private void Reset()
    {
        button = GetComponent<Button>();
        titleText = GetComponentInChildren<TMP_Text>();
    }

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        shop = GetComponentInParent<ShopUI>();
        if (shop == null)
            Debug.LogError($"[ShopItemButton] No ShopUI found in parent for '{name}'.");
    }

    public void Init(BlockData data)
    {
        block = data;
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        if (costText != null)
            costText.text = block.cost.ToString();
        
        titleText.text = block.alias;
        
        if (purchasedBadge != null)
            purchasedBadge.SetActive(block.bought);

        SetPurchasedVisual(block.bought);
    }

    private void OnClick()
    {
        if (shop != null)
            shop.TryPurchase(this);
    }

    public void RefreshAffordableState(int currentCoins)
    {
        bool affordable = currentCoins >= block.cost;
        if (button != null)
            button.interactable = true;
        if (costText != null)
            costText.color = affordable ? Color.green : Color.red;
    }

    public void SetPurchasedVisual(bool purchased)
    {
        if (purchasedBadge != null)
            purchasedBadge.SetActive(purchased);
    }

    public void PulseNotEnough()
    {
        Debug.Log($"[ShopItemButton] Not enough coins for '{block.alias}'. Cost {block.cost}.");
    }
}
