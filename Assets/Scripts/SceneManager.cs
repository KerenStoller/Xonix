using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void OpenGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}