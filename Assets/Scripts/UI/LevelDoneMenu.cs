using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDoneMenu : MonoBehaviour
{
    [Header("Buttons")][SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text rewardText;

    [Header("Next Level")]

    private AudioSource winSound;
    private bool winSoundPlayed = false;

    void Awake()
    {
        winSound = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (GameManager.Instance.currentLevel < GameManager.Instance.levels.Count -1){
            nextLevelButton.gameObject.SetActive(true);
            if (nextLevelButton != null)
                nextLevelButton.onClick.AddListener(() => GameManager.Instance.LoadLevel(GameManager.Instance.currentLevel + 1));
        }
        else{
            nextLevelButton.gameObject.SetActive(false);
        }

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(MainMenu);

        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManager.Instance.LoadLevel(GameManager.Instance.currentLevel));

        if (GameManager.Instance != null)
            GameManager.Instance.LevelWon += PlayWinSound;
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        rewardText.text = "Reward: " + GameManager.Instance.level.reward;
    }

    void OnDisable()
    {
        GameManager.Instance.LevelWon -= PlayWinSound;
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
