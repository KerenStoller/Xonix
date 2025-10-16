using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Grid grid;
    [SerializeField] private Text percentageText;
    [SerializeField] private GameObject lifePrefab;
    [SerializeField] private Transform livesContainer;
    
    private List<GameObject> _lives = new List<GameObject>();
    public GameResult gameResult;
    private Tilemap _grassTilemap;
    private Tilemap _groundTilemap;
    
    [SerializeField] private float grassThreshold = 0.7f; // percentage needed to win
    [SerializeField] private int chickenMaxLives = 3;
    [SerializeField] private float timeToCheck = 0.35f;
    
    private int _chickenCurrentLives;
    private float _moveCountdown;
    private bool _won;

    private void Awake()
    {
        _grassTilemap = grid.transform.Find("Grass").GetComponent<Tilemap>();
        _groundTilemap = grid.transform.Find("Ground").GetComponent<Tilemap>();
        
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        _chickenCurrentLives = chickenMaxLives;
        InitializeLives(_chickenCurrentLives);
        percentageText.text = "0%";
    }
    
    public void InitializeLives(int lives)
    {
        // Destroy extra hearts
        while (_lives.Count > lives)
        {
            Destroy(_lives[_lives.Count - 1]);
            _lives.RemoveAt(_lives.Count - 1);
        }
        // Add needed _lives
        while (_lives.Count < lives)
        {
            var life = Instantiate(lifePrefab, livesContainer);
            _lives.Add(life);
        }
    }
    
    void Update()
    {
        if (!_won)
        {
            _moveCountdown += Time.deltaTime;

            if (_moveCountdown >= timeToCheck)
            {
                _moveCountdown = 0;
                if (!_won)
                {
                    IsGrassCoverageAboveThreshold();
                }
                if (_won)
                {
                    gameResult.WhoWon = "Chick";
                    UnityEngine.SceneManagement.SceneManager.LoadScene("WonOrLost", LoadSceneMode.Single);
                }
            }   
        }
    }

    private void IsGrassCoverageAboveThreshold()
    {
        int grassCount = 0;
        int totalCount = 0;
        BoundsInt bounds = _groundTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            totalCount++;
            if (_grassTilemap.HasTile(pos))
                grassCount++;
        }
        
        float coverage = (float)grassCount / totalCount;
        percentageText.text = $"{coverage * 100:0}%";
        _won = coverage >= grassThreshold;
    }
    
    public void SetLives(int currentLives)
    {
        for (int i = 0; i < _lives.Count; i++)
        {
            _lives[i].SetActive(i < currentLives);
        }
    }
    
    public void RemoveLife()
    {
        _chickenCurrentLives--;
        if (_chickenCurrentLives <= 0)
        {
            
            gameResult.WhoWon = "Cows";
            UnityEngine.SceneManagement.SceneManager.LoadScene("WonOrLost", LoadSceneMode.Single);
        }
        else
        {
            SetLives(_chickenCurrentLives);
        }
    }
}