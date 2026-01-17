using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SimulateButton : MonoBehaviour
{
    private bool simulating = false;
    private TMP_Text text;

    private InputAction simulateHotkey;

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        
        GameManager.Instance.OnSimulationEnd += InstanceOnonSimulationEnd;
        GameManager.Instance.OnSimulationStart += InstanceOnOnSimulationStart;

        simulateHotkey = InputSystem.actions.FindAction("Toggle_simulation");
        
        simulateHotkey.performed += OnHotkeyPressed;

    }

    private void OnDestroy()
    {
        simulateHotkey.performed -= OnHotkeyPressed;
    }


    // Start simulation hotkey
    void OnHotkeyPressed(InputAction.CallbackContext context)
    {
        OnButtonClick();
    }

    private void InstanceOnOnSimulationStart()
    {
        simulating = true;
        text.text = "Stop simulating";
    }

    private void InstanceOnonSimulationEnd()
    {
        simulating = false;
        text.text = "Start simulation";        
    }

    private void OnButtonClick()
    {
        if  (!simulating)
            GameManager.Instance.RaiseSimulationStart();
        else
            GameManager.Instance.RaiseSimulationEnd();
    }


}
