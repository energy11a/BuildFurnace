using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Data;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
    [Header("Level Setup")]
    [SerializeField] private Transform levelButtonsParent; // Alamelement, mis sisaldab kõiki level nuppe
    [SerializeField] private int levelsPerPage = 6;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button shopButton;

    [Header("LevelButton Prefab")]
    [SerializeField] private LevelButton levelButtonPrefab;

    private List<GameObject> levelButtons = new List<GameObject>();
    private GameManager _gameManager;
    private int currentPage = 0;
    private int totalPages = 0;

    void Start()
    {
        _gameManager = GameManager.Instance;
        for (int i = 0; i < _gameManager.levels.Count; i++)
        {
            LevelButton button = Instantiate(levelButtonPrefab, levelButtonsParent);
            button.Init(i);
            levelButtons.Add(button.gameObject);
        }
        totalPages = Mathf.CeilToInt((float)_gameManager.levels.Count / levelsPerPage);

        // Nuppude listenerid
        backButton.onClick.AddListener(BackToMenu);
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        shopButton.onClick.AddListener(OpenShop);

        UpdatePage();
    }

    void UpdatePage()
    {
        int startIndex = currentPage * levelsPerPage;
        int endIndex = startIndex + levelsPerPage;

        // N�ita ainult praegusel lehel olevaid nuppe
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].SetActive(i >= startIndex && i < endIndex);
        }

        // Noolte n�htavus
        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive(currentPage < totalPages - 1);
    }

    public void NextPage()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
