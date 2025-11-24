
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string levelSceneName;
    [Header("Audio")]
    [SerializeField] private AudioClip startLevelSound;
    private AudioSource audioSource;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        if (button != null)
            button.onClick.AddListener(OpenLevel);
    }

    private void OpenLevel()
    {
        if (startLevelSound && audioSource)
        {
            audioSource.PlayOneShot(startLevelSound);
            DontDestroyOnLoad(audioSource.gameObject);
            StartCoroutine(DestroyAfterClip(startLevelSound.length));
        }

        SceneManager.LoadSceneAsync(levelSceneName, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("NeededInAll", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }

    private System.Collections.IEnumerator DestroyAfterClip(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(audioSource.gameObject);
    }
}
