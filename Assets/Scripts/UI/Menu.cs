using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Selle saad määrata Inspectoris, et teada, mis scene avada Play nupu vajutamisel
    [SerializeField] private string levelSelectSceneName = "LevelSelect";
    [SerializeField] private GameObject settingsMenu; // viide seadete menüüle

    // Play nupp
    public void PlayGame()
    {
        SceneManager.LoadScene(levelSelectSceneName);
    }

    // Settings nupp
    public void OpenSettings()
    {
        if (settingsMenu != null)
            settingsMenu.SetActive(true);
    }

    // Close Settings (vajadusel tagasinupu jaoks)
    public void CloseSettings()
    {
        if (settingsMenu != null)
            settingsMenu.SetActive(false);
    }

    // Quit nupp
    public void QuitGame()
    {
        Debug.Log("M�ng suletud!");
        Application.Quit();

        // Editoris testimiseks (Unity Editoris ei t��ta Application.Quit)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
