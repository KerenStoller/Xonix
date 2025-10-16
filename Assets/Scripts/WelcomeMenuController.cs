using UnityEngine;

public class WelcomeMenuController : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject startButton;
    public GameObject instructionsButton;
    public GameObject gameName;

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        startButton.SetActive(false);
        instructionsButton.SetActive(false);
        gameName.SetActive(false);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        startButton.SetActive(true);
        instructionsButton.SetActive(true);
        gameName.SetActive(true);
    }
}
