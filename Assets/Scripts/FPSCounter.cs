using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text text;
    private int fps;
    [SerializeField] private float refreshRate = 1f;

    private float timer;

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            fps = (int)(1f / Time.unscaledDeltaTime);
            text.text = "FPS: " + fps;
            timer = Time.unscaledTime + refreshRate;
        }
    }
}
