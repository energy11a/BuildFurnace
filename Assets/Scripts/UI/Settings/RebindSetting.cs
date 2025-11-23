using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class RebindSetting : MonoBehaviour
{
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text btnText;
    [SerializeField] private GameObject waitForInputScreen;

    private bool currentlyChanging = false;
    private string path;

    private void Start()
    {

        if (btnText == null && inputAction == null)
        {
            Debug.LogError("Missing references!");
            return;
        }

        if (btn != null) 
        {
            btn.onClick.AddListener(delegate 
            {
                StartChange();
            });
        }

        ChangeBtnText();

    }


    void StartChange() 
    {
        path = inputAction.action.bindings[0].effectivePath; //  <Keyboard>/e
        Debug.Log("Current path: " + path);
        currentlyChanging = true;
        waitForInputScreen.SetActive(true);
    }

    private void OnGUI()
    {
        if (!currentlyChanging) return;

        Event e = Event.current;
        if (e.isKey) 
        {
            Debug.Log("Detected key code: " + e.keyCode + " Name: " + e.ToString());
            
            Change(e.keyCode.ToString());
            currentlyChanging = false;
        }
    }

    void Change(string s) 
    {
        string[] strings = path.Split("/");
        string newPath = strings[0] + "/" + s.ToLower();
        inputAction.action.ApplyBindingOverride(newPath);
        waitForInputScreen.SetActive(false);
        ChangeBtnText();
    }

    void ChangeBtnText() 
    {
        btnText.text = inputAction.action.bindings[0].effectivePath.Split("/")[1];
        
    }

}
