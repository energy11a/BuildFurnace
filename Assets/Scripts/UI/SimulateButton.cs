using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimulateButton : MonoBehaviour
{
    private bool simulating = false;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        
        Events.Instance.OnSimulationEnd += InstanceOnonSimulationEnd;
        Events.Instance.OnSimulationStart += InstanceOnOnSimulationStart;
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
            Events.Instance.RaiseSimulationStart();
        else
            Events.Instance.RaiseSimulationEnd();
    }
}
