using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;      // Canvas / panel pausimen�� jaoks
    [SerializeField] private GameObject levelWonPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button levelsButton;
    [SerializeField] private GameObject settingsMenu;

    private bool isPaused = false;

    void Start()
    {
        Events.Instance.LevelWon += OnLevelEnd;
        // Pane pausimen�� alguses kinni
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

   private void OnLevelEnd()
    {
        Time.timeScale = 0f;
        levelWonPanel.SetActive(true);
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

        Time.timeScale = 0f; // peatab m�ngu
        AudioListener.pause = true; // peatab heli
    }

    private void ResumeGame()
    {
        isPaused = false;

        if (settingsMenu != null)
            settingsMenu.SetActive(false);

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void OpenSettings()
    {
        Debug.Log("Avatud seadete men��!");
        // V�id siin aktiveerida settings men�� UI
        if (settingsMenu != null)
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }
    }

    private void OpenMainMenu()
    {
        Time.timeScale = 1f; // veendu, et m�ng ei j�� pausile
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu"); // pane siia oma men�� scene
    }

    private void OpenLevels()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("LevelSelect"); // pane siia oma level select scene
    }
}
