using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Grid grid;
    [SerializeField] private Text percentageText;
    [SerializeField] private Text winText;
    [SerializeField] private Text livesText;
    
    private Tilemap _grassTilemap;
    private Tilemap _groundTilemap;

    private const int ChickenMaxLives = 3;
    private int _chickenCurrentLives;
    [SerializeField] private float timeToCheck = 0.35f;
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
        _chickenCurrentLives = ChickenMaxLives;
        livesText.text = _chickenCurrentLives.ToString();
        percentageText.text = "0%";
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
                    winText.gameObject.SetActive(true);
                    Chicken.Instance.transform.gameObject.SetActive(false);
                }
            }   
        }
    }

    private void IsGrassCoverageAboveThreshold()
    {
        float threshold = 0.75f;
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
        _won = coverage >= threshold;
    }
    
    public void RemoveLife()
    {
        _chickenCurrentLives--;
        livesText.text = _chickenCurrentLives.ToString();
        Debug.Log($"Chicken lost a life, remaining lives: {_chickenCurrentLives}");
        if (_chickenCurrentLives <= 0 )
        {
            // game is over
        }
    }
}