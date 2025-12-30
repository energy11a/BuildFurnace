using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDoneMenu : MonoBehaviour
{
    [Header("Buttons")][SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    [Header("Next Level")]

    private AudioSource winSound;
    private bool winSoundPlayed = false;

    void Awake()
    {
        winSound = GetComponent<AudioSource>();
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
        Events.Instance.LevelWon -= PlayWinSound;
    }
    private void PlayWinSound()
    {
        if (winSoundPlayed) return;
        winSoundPlayed = true;

        if (winSound != null)
        {
            winSound.loop = false;
            winSound.ignoreListenerPause = true; // play even if AudioListener.pause is true
            winSound.Play();
        }
    }
}
