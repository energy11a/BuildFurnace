using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text FpsText;

    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    private void Start()
    {
        gameObject.SetActive(SettingsData.showFPS);
        FpsText.text = "";
    }

    private void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if (time >= pollingTime) {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = frameRate.ToString() + " FPS";

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
