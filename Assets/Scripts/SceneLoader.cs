using Unity.VisualScripting;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public GameResult gameResult;
    [SerializeField] private GameObject WonGrid;
    [SerializeField] private GameObject LostGrid;
    [SerializeField] private GameObject WonText;
    [SerializeField] private GameObject LostText;
    [SerializeField] private GameObject PlayAgainButton;
    [SerializeField] private GameObject MenuButton;
    
    
    void Start()
    {
        PlayAgainButton.SetActive(true);
        MenuButton.SetActive(true);
        
        if (gameResult.WhoWon == "")
        {
            WonGrid.SetActive(false);
            WonText.SetActive(false);
            LostGrid.SetActive(false);
            LostText.SetActive(false);
        }
        if (gameResult.WhoWon == "Chick")
        {
            WonGrid.SetActive(true);
            WonText.SetActive(true);
            LostGrid.SetActive(false);
            LostText.SetActive(false);
        }
        else
        {
            WonGrid.SetActive(false);
            WonText.SetActive(false);
            LostGrid.SetActive(true);
            LostText.SetActive(true);
        }
    }
}