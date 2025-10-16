using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameResult gameResult;
    [SerializeField] private GameObject wonGrid;
    [SerializeField] private GameObject lostGrid;
    [SerializeField] private GameObject wonText;
    [SerializeField] private GameObject lostText;
    [SerializeField] private GameObject playAgainButton;
    [SerializeField] private GameObject menuButton;
    
    
    private void Start()
    {
        playAgainButton.SetActive(true);
        menuButton.SetActive(true);
        
        if (gameResult.whoWon == "")
        {
            wonGrid.SetActive(false);
            wonText.SetActive(false);
            lostGrid.SetActive(false);
            lostText.SetActive(false);
        }
        if (gameResult.whoWon == "Chick")
        {
            wonGrid.SetActive(true);
            wonText.SetActive(true);
            lostGrid.SetActive(false);
            lostText.SetActive(false);
        }
        else
        {
            wonGrid.SetActive(false);
            wonText.SetActive(false);
            lostGrid.SetActive(true);
            lostText.SetActive(true);
        }
    }
}