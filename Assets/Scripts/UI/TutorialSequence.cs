using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialSequence : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Transform windowContainer;
    public TextMeshProUGUI tutorialText;
    public Button closeButton;
    public string tutorialSceneName = "Tutorial";
    IEnumerator Start()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        yield return new WaitForSecondsRealtime(0.3f);

        if (SceneManager.GetActiveScene().name != tutorialSceneName)
        {
            Destroy(gameObject);
            yield break;
        }

        if (tutorialPanel != null) tutorialPanel.SetActive(true);

        Time.timeScale = 0f;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseTutorial);

        StartCoroutine(AnimatePop());
    }

    IEnumerator AnimatePop()
    {
        float timer = 0;
        while (timer < 0.2f)
        {
            timer += Time.unscaledDeltaTime;
            float scale = Mathf.Lerp(0.8f, 1f, timer / 0.2f);
            if (windowContainer != null) windowContainer.localScale = Vector3.one * scale;
            yield return null;
        }
        if (windowContainer != null) windowContainer.localScale = Vector3.one;
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1f;
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        Destroy(gameObject);
    }
}