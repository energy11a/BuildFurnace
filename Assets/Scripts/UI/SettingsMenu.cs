using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button settingsButton;

    [Header("Tabs")]
    [SerializeField] private GameObject audioTab;
    [SerializeField] private GameObject graphicsTab;
    [SerializeField] private GameObject controlsTab;

    [Header("TabButton")]
    [SerializeField] private Button audioBtn;
    [SerializeField] private Button graphicsBtn;
    [SerializeField] private Button controlsBtn;

    [Header("UI Sliders")]
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer; // viide MainAudioMixer

    private void Start()
    {
        // Kui sliderid on olemas, ühenda listenerid
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Algväärtus slideritele
        float musicVol, sfxVol;
        if (audioMixer.GetFloat("Music", out musicVol))
            musicSlider.value = Mathf.Pow(10, musicVol / 20f); // log -> linear

        if (audioMixer.GetFloat("SFX", out sfxVol))
            sfxSlider.value = Mathf.Pow(10, sfxVol / 20f);
        // Nupud
        if (settingsButton != null)
            settingsButton.onClick.AddListener(Close);
    }

    public void SetMusicVolume(float value)
    {
        // Slideri väärtus 0-1 -> AudioMixer dB
        float dB = Mathf.Clamp(-80f, 0f, value);
        audioMixer.SetFloat("Music", dB);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Clamp(-80f, 0f, value);
        audioMixer.SetFloat("SFX", dB);
    }
    public void Close()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            Debug.Log("Avatud seadete menüü!");
            settingsMenu.SetActive(false);
        }
    }
}
