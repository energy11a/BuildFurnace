using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GraphicsSettings : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown resDropdown;

    bool isFullscreen = false;
    private int width = 0;
    private int height = 0;



    private void Start()
    {
        if (resDropdown != null) 
        {
            resDropdown.onValueChanged.AddListener(delegate
            {
                ChangeRes(resDropdown.value);
            });
        }
    }

    public void ChangeRes(int value) 
    {


        //width = w;
        //height = h;
        //Screen.SetResolution(w, h, isFullscreen);
    }

    
    public void SetFullscreen(bool state) 
    {
        isFullscreen = state;
        Screen.fullScreen = state;
    }


    public void SetFps(int amount) 
    {
        if (amount <= 5) 
        {
            Debug.Log("FPS set too low!");
        }
        Application.targetFrameRate = amount;
    }



}
