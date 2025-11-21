using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button closeButton;

    [Header("Tabs")]
    [SerializeField] private GameObject audioTab;
    [SerializeField] private GameObject graphicsTab;
    [SerializeField] private GameObject controlsTab;

    [Header("TabButton")]
    [SerializeField] private Button audioBtn;
    [SerializeField] private Button graphicsBtn;
    [SerializeField] private Button controlsBtn;



}