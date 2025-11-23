using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] private GameObject audioTab;
    [SerializeField] private GameObject graphicsTab;
    [SerializeField] private GameObject controlsTab;

    [Header("TabButton")]
    [SerializeField] private Button audioBtn;
    [SerializeField] private Button graphicsBtn;
    [SerializeField] private Button controlsBtn;

    [SerializeField] private Button backButton;
    [SerializeField] private GameObject pauseMenu;

    private GameObject currentlyActive;

    private void Start()
    {
        audioBtn.onClick.AddListener(() => SwitchTo(audioTab));
        graphicsBtn.onClick.AddListener(() => SwitchTo(graphicsTab));
        controlsBtn.onClick.AddListener(() => SwitchTo(controlsTab));
        backButton.onClick.AddListener(CloseSettings);

        controlsTab.SetActive(false);
        graphicsTab.SetActive(false);

        SwitchTo(audioTab);
        

        


    }



    // Switches to tab
    private void SwitchTo(GameObject tab) 
    {
        if (currentlyActive != null)
        {
            currentlyActive.SetActive(false);
        }

        tab.SetActive(true);

        currentlyActive = tab;
    }



    public void OpenSettings()
    {
        gameObject.SetActive(true);
    }

    public void CloseSettings() 
    {
        gameObject.SetActive(false);
        if (pauseMenu != null)
            pauseMenu.SetActive(true);
    }

    

}