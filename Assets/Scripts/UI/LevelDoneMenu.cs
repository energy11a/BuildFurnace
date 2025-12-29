using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDoneMenu : MonoBehaviour
{
    [Header("Buttons")][SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    [Header("Next Level")]

    [SerializeField] private AudioClip winSound;
    private AudioSource audioSource;
    private bool winSoundPlayed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (Events.Instance.currentLevel < Events.Instance.levels.Count -1){
            nextLevelButton.gameObject.SetActive(true);
            if (nextLevelButton != null)
                nextLevelButton.onClick.AddListener(() => Events.Instance.LoadLevel(Events.Instance.currentLevel + 1));
        }
        else{
            nextLevelButton.gameObject.SetActive(false);
        }

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(MainMenu);

        if (restartButton != null)
            restartButton.onClick.AddListener(() => Events.Instance.LoadLevel(Events.Instance.currentLevel));

        if (Events.Instance != null)
            Events.Instance.LevelWon += PlayWinSound;
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
