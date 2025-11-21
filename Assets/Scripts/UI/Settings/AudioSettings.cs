using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSettings : MonoBehaviour
{
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
    
}


