using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
  [SerializeField] private string firstSceneName = "MainMenu";

  private void Awake()
  {
    Application.targetFrameRate = 144;
    if (GameManager.Instance == null)
    {
      var eventsGO = new GameObject("Events");
      eventsGO.AddComponent<GameManager>();
    }

    if (Wallet.Instance == null)
    {
      var walletGO = new GameObject("Wallet");
      walletGO.AddComponent<Wallet>();
    }

    if (!string.IsNullOrEmpty(firstSceneName))
    {
      SceneManager.LoadScene(firstSceneName, LoadSceneMode.Single);
    }
    else
    {
      Debug.LogError("[Bootstrapper] firstSceneName is empty. Set it in the Inspector.");
    }
  }
}
