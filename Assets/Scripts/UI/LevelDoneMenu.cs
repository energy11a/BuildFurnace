using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDoneMenu : MonoBehaviour
{
    [Header("Buttons")] [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    [Header("Next Level")] [SerializeField]
    private string nextLevelSceneName; // Inspectoris m��ratav level, mis avatakse Next Level nupuga

    void Start()
    {
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(NextLevel);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(MainMenu);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);
    }


    private void NextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            SceneManager.LoadScene(nextLevelSceneName);
            SceneManager.LoadSceneAsync("NeededInAll", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("UI", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("NextLevel nupp ei ole seotud scene'iga!");
        }
    }

    private void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync("NeededInAll", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Single);
    }

    private void MainMenu()
    {
        SceneManager.sceneLoaded += ResetTimeScale;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void ResetTimeScale(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Debug.Log($"[DEBUG] Time.timeScale reset after loading {scene.name}");
        SceneManager.sceneLoaded -= ResetTimeScale;
    }
}