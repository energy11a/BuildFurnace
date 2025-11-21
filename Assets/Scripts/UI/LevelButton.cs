using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string levelSceneName; // Scene nimi, mida see nupp avab

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OpenLevel);
    }

    void OpenLevel()
    {
        if (!string.IsNullOrEmpty(levelSceneName))
        {
            SceneManager.LoadSceneAsync(levelSceneName,LoadSceneMode.Single );
            SceneManager.LoadSceneAsync("NeededInAll", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("UI",LoadSceneMode.Additive);
            
        }
        else
        {
            Debug.LogWarning("LevelButton: levelSceneName ei ole määratud!");
        }
    }
}
