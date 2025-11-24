using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDoneMenu : MonoBehaviour
{
    [Header("Buttons")][SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    [Header("Next Level")]
    [SerializeField]
    private string nextLevelSceneName; // Inspectoris m��ratav level, mis avatakse Next Level nupuga

    [SerializeField] private AudioClip winSound;
    private AudioSource audioSource;
    private bool winSoundPlayed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(NextLevel);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(MainMenu);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);

        if (Events.Instance != null)
            Events.Instance.LevelWon += PlayWinSound;
    }

    private void NextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            SceneManager.sceneLoaded += ResetTimeScale;
            SceneManager.LoadSceneAsync(nextLevelSceneName, LoadSceneMode.Single);
            SceneManager.LoadSceneAsync("NeededInAll", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        }
    }

    private void RestartLevel()
    {
        SceneManager.sceneLoaded += ResetTimeScale;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(currentScene.name, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("NeededInAll", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
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

    void OnDisable()
    {
        if (Events.Instance != null)
            Events.Instance.LevelWon -= PlayWinSound;
    }
    private void PlayWinSound()
    {
        if (winSoundPlayed) return;
        winSoundPlayed = true;

        if (winSound != null && audioSource != null)
        {
            audioSource.loop = false;
            audioSource.ignoreListenerPause = true; // play even if AudioListener.pause is true
            audioSource.PlayOneShot(winSound);
        }
    }
}