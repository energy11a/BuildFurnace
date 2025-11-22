using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GraphicsSettings : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown resDropdown;
    [SerializeField] private TMP_InputField fpsInputField;

    // List of resolutions, Width X Height
    List<string> resOptions = new List<string> {"1920x1080", "1600x900", "1280x800", "800x600" };

    bool isFullscreen = false;
    
    private void Start()
    {
        if (resDropdown != null) 
        {
            resDropdown.onValueChanged.AddListener(delegate
            {
                ChangeRes(resDropdown.value);
            });
            // Add the resolutions
            resDropdown.ClearOptions();
            resDropdown.AddOptions(resOptions);
        }

        if (fpsInputField != null) 
        {
            fpsInputField.onEndEdit.AddListener(delegate
            {
                SetFps(int.Parse(fpsInputField.text));
            });
        }

    }

    public void ChangeRes(int value) 
    {
        string resString = resOptions[value];
        string[] strings = resString.Split("x");

        int width = int.Parse(strings[0]);
        int height = int.Parse(strings[1]);
        Screen.SetResolution(width, height, isFullscreen);
        
    }

    
    public void SetFullscreen(bool state) 
    {
        isFullscreen = state;
        Screen.fullScreen = state;
    }


    public void SetFps(int amount) 
    {
        if (amount <= 4) 
        {
            Debug.Log("FPS set too low!");
            return;
        }
        Application.targetFrameRate = amount;
        Debug.Log(amount);
    }



}
