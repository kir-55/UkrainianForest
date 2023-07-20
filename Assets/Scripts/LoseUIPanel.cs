using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseUIPanel : MonoBehaviour
{
    public void Donate()
    {
        Application.OpenURL("https://savelife.in.ua");
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
