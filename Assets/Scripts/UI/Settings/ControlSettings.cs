using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlSettings : MonoBehaviour
{
    [SerializeField] Button camRightBtn;
    [SerializeField] Button camLeftBtn;
    [SerializeField] Button startSimBtn;
    [SerializeField] Button pauseBtn;



    private void Start()
    {
        Debug.Log(InputSystem.actions.actionMaps[2]);  
    }

}
