
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private string itemId = "ExtraBlock";
    [SerializeField] private int cost = 50;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private GameObject purchasedBadge;

    private ShopUI shop;

    public string ItemId => itemId;
    public int Cost => cost;

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

    private void OnEnable()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        if (costText != null)
            costText.text = cost.ToString();

        if (purchasedBadge != null)
            purchasedBadge.SetActive(TempInventory.HasItem(itemId));

        SetPurchasedVisual(TempInventory.HasItem(itemId));
    }

    private void OnClick()
    {
        if (shop != null)
            shop.TryPurchase(this);
    }

    public void RefreshAffordableState(int currentCoins)
    {
        bool affordable = currentCoins >= cost;
        if (button != null)
            button.interactable = true;
        if (costText != null)
            costText.color = affordable ? Color.white : new Color(1f, 0.5f, 0.5f);
    }

    public void SetPurchasedVisual(bool purchased)
    {
        if (purchasedBadge != null)
            purchasedBadge.SetActive(purchased);
    }

    public void PulseNotEnough()
    {
        Debug.Log($"[ShopItemButton] Not enough coins for '{itemId}'. Cost {cost}.");
    }
}
