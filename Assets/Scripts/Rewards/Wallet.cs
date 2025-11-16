using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    private const string CoinsKey = "Coins";
    public int Coins { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Coins = PlayerPrefs.GetInt(CoinsKey, 0);
        Events.Instance?.RaiseCoinsChanged(Coins);
    }

    public void Add(int amount)
    {
        if (amount <= 0) return;
        Coins += amount;
        Save();
        Events.Instance?.RaiseCoinsChanged(Coins);
    }

    public bool Spend(int amount)
    {
        if (amount <= 0) return true;
        if (Coins < amount) return false;

        Coins -= amount;
        Save();
        Events.Instance?.RaiseCoinsChanged(Coins);
        return true;
    }

    public void ResetToZero()
    {
        Coins = 0;
        Save();
        Events.Instance?.RaiseCoinsChanged(Coins);
    }

    private void Save()
    {
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}