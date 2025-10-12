using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void OpenGameScene()
    {
        Debug.Log("Opening game scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

}
