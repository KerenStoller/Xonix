using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Text percentageText;
    [SerializeField] private Text winText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Chicken chicken;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private Tilemap flowerMap;

    private Vector3[] cowStartPositions;



    private bool _isGameOver = false;
    [SerializeField] private float timeToCheck = 0.35f;
    private float _moveCountdown;
    private bool _won;

    private Dictionary<Vector3Int, TileBase> _initialGrassTiles = new Dictionary<Vector3Int, TileBase>();
    private Dictionary<Vector3Int, TileBase> _initialGroundTiles = new Dictionary<Vector3Int, TileBase>();
    private bool _cachedTiles;

    private void Awake()
    {
        if (!_cachedTiles)
        {
            CacheInitialTilemaps();
            _cachedTiles = true;
        }
        var cows = GameObject.FindGameObjectsWithTag("Cow");
        cowStartPositions = new Vector3[cows.Length];
        for (int i = 0; i < cows.Length; i++)
        {
            cowStartPositions[i] = cows[i].transform.position;
        }

    }

    private void CacheInitialTilemaps()
    {
        _initialGrassTiles.Clear();
        _initialGroundTiles.Clear();

        foreach (var pos in grassTilemap.cellBounds.allPositionsWithin)
        {
            if (grassTilemap.HasTile(pos))
                _initialGrassTiles[pos] = grassTilemap.GetTile(pos);
        }

        foreach (var pos in groundTilemap.cellBounds.allPositionsWithin)
        {
            if (groundTilemap.HasTile(pos))
                _initialGroundTiles[pos] = groundTilemap.GetTile(pos);
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
                IsGrassCoverageAboveThreshold();
                if (_won)
                {
                    winText.gameObject.SetActive(true);
                    PlayButton.SetActive(true);
                    _isGameOver = true;
                    if (chicken != null)
                        chicken.gameObject.SetActive(false);
                    Time.timeScale = 0f;

                }
            }
        }
    }

    private void IsGrassCoverageAboveThreshold()
    {
        float threshold = 0.75f;
        int grassCount = 0;
        int totalCount = 0;
        BoundsInt bounds = groundTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            totalCount++;
            if (grassTilemap.HasTile(pos))
                grassCount++;
        }

        if (totalCount > 0)
        {
            float coverage = (float)grassCount / totalCount;
            percentageText.text = $"{coverage * 100:0}%";
            _won = coverage > threshold;
        }
        else
        {
            percentageText.text = "0%";
        }
    }

    private void OnEnable()
    {
        Chicken.OnPlayerDied += TriggerGameOver;
    }

    private void OnDisable()
    {
        Chicken.OnPlayerDied -= TriggerGameOver;
    }

    private void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        PlayButton.SetActive(true);

        if (chicken != null)
            chicken.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    public void Play()
    {
        Debug.Log("Play button pressed");
        ResetGame();
    }

    private void ResetGame()
    {
        gameOverPanel.SetActive(false);
        PlayButton.SetActive(false);
        winText.gameObject.SetActive(false);

        chicken.gameObject.SetActive(true);
        chicken.ResetState();
        Debug.Log("Reseted chicken state it now has 3 lives after restart button");

       var cows = GameObject.FindGameObjectsWithTag("Cow");
        for (int i = 0; i < cows.Length && i < cowStartPositions.Length; i++)
        {
            cows[i].transform.position = cowStartPositions[i];
        }


        RestoreTilemaps();

        percentageText.text = "0%";
        _won = false;
        _isGameOver = false;
        Time.timeScale = 1f;
    }

    private void RestoreTilemaps()
    {
        grassTilemap.ClearAllTiles();
        groundTilemap.ClearAllTiles();
        flowerMap.ClearAllTiles();

        foreach (var kvp in _initialGroundTiles)
            groundTilemap.SetTile(kvp.Key, kvp.Value);

        foreach (var kvp in _initialGrassTiles)
            grassTilemap.SetTile(kvp.Key, kvp.Value);
    }
}
