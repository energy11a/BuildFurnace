
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int level;
    private Button button;

    public void Init(int i){
        this.level = i;
        GetComponentInChildren<TMP_Text>().text = GameManager.Instance.levels[i].name;
    }

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(() => GameManager.Instance.LoadLevel(level));
    }
}
