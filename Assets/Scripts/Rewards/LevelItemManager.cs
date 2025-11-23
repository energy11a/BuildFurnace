using System.Linq;
using UnityEngine;

public class LevelItemManager : MonoBehaviour
{
    [Header("Extra Block")]
    [SerializeField] private GameObject extraBlockPrefab;
    [SerializeField] private Transform extraBlockSpawnPoint;

    private bool appliedThisLevel;

    private void OnEnable()
    {
        if (Events.Instance != null)
        {
            Events.Instance.OnSimulationStart += ApplyItemsForLevel;
            Events.Instance.LevelWon += ClearAfterLevel;
            Events.Instance.OnSimulationEnd += ClearAfterLevel;
        }
    }

    private void OnDisable()
    {
        if (Events.Instance != null)
        {
            Events.Instance.OnSimulationStart -= ApplyItemsForLevel;
            Events.Instance.LevelWon -= ClearAfterLevel;
            Events.Instance.OnSimulationEnd -= ClearAfterLevel;
        }
    }

    private void ApplyItemsForLevel()
    {
        if (appliedThisLevel) return; // apply once per level start
        appliedThisLevel = true;

        Debug.Log("[LevelItemManager] Applying purchased items…");

        int extraBlockCount = TempInventory.PurchasedItems.Count(id => id == "ExtraBlock");
        for (int i = 0; i < extraBlockCount; i++)
            SpawnExtraBlock(i);
    }

    private void SpawnExtraBlock(int index)
    {
        if (extraBlockPrefab == null)
        {
            Debug.LogWarning("[LevelItemManager] No extraBlockPrefab assigned.");
            return;
        }

        Vector3 basePos = extraBlockSpawnPoint ? extraBlockSpawnPoint.position : Vector3.zero;
        // Offset blocks slightly so they don’t overlap
        var pos = basePos + new Vector3(index * 1.0f, 0f, 0f);
        Instantiate(extraBlockPrefab, pos, Quaternion.identity);
        Debug.Log($"[LevelItemManager] Spawned ExtraBlock #{index + 1} at {pos}");
    }

    private void ClearAfterLevel()
    {
        Debug.Log("[LevelItemManager] Clearing TempInventory after level.");
        TempInventory.Clear();
        appliedThisLevel = false;
    }
}
