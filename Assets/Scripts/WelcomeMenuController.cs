using UnityEngine;

public class WelcomeMenuController : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject startButton;
    public GameObject instructionsButton;
    public GameObject GameName;

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        startButton.SetActive(false);          // Hide Start button
        instructionsButton.SetActive(false);   // Hide Instructions button
        GameName.SetActive(false);             // Hide Game Name
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        startButton.SetActive(true);           // Show Start button
        instructionsButton.SetActive(true);    // Show Instructions button
        GameName.SetActive(true);              // Show Game Name
    }
}
