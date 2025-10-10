using UnityEngine;

public class SceneManager : MonoBehaviour
{

    private bool _isGameOver = false;

    public void OpenGameScene()
    {
        Debug.Log("Opening game scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

}
