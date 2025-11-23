using System.Collections.Generic;

public static class TempInventory
{
    public static readonly List<string> PurchasedItems = new List<string>();

    public static void AddItem(string itemId)
    {
        if (!PurchasedItems.Contains(itemId))
            PurchasedItems.Add(itemId);
    }

    public static bool HasItem(string itemId) => PurchasedItems.Contains(itemId);

    public static void Clear() => PurchasedItems.Clear();
}
