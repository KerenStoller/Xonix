using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void OpenGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    
    public void OpenWelcomeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("WelcomeScene");
    }
}