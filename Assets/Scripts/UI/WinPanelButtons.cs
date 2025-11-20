using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanelController : MonoBehaviour
{
    [Header("Buttons (assign in Inspector)")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string nextLevelSceneName = "";
    private void Awake()
    {
        if (gameObject.activeSelf) gameObject.SetActive(false);

        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
        if (restartButton != null) restartButton.onClick.AddListener(RestartLevel);
        if (nextLevelButton != null) nextLevelButton.onClick.AddListener(LoadNextLevel);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void GoToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        else
            Debug.LogError("[WinPanelController] MainMenu scene name is empty.");
    }

    private void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelSceneName))
            SceneManager.LoadScene(nextLevelSceneName, LoadSceneMode.Single);
        else
            Debug.LogWarning("[WinPanelController] Next level scene name not set.");
    }
}