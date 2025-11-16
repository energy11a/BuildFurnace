using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;      // Canvas / panel pausimenüü jaoks
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button levelsButton;
    [SerializeField] private GameObject settingsMenu;

    private bool isPaused = false;

    void Start()
    {
        // Pane pausimenüü alguses kinni
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        // Nupud
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OpenMainMenu);

        if (levelsButton != null)
            levelsButton.onClick.AddListener(OpenLevels);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        Time.timeScale = 0f; // peatab mängu
        AudioListener.pause = true; // peatab heli
    }

    private void ResumeGame()
    {
        isPaused = false;
        if (pauseMenu != null)
            if (settingsMenu != null)
                settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void OpenSettings()
    {
        Debug.Log("Avatud seadete menüü!");
        // Võid siin aktiveerida settings menüü UI
        if (settingsMenu != null)
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }
            
            
    }

    private void OpenMainMenu()
    {
        Time.timeScale = 1f; // veendu, et mäng ei jää pausile
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu"); // pane siia oma menüü scene
    }

    private void OpenLevels()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("LevelSelect"); // pane siia oma level select scene
    }
}
