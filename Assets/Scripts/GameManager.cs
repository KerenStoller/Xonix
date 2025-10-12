using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Text percentageText;
    [SerializeField] private Text winText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Chicken chicken;
    [SerializeField] private GameObject PlayButton;
    private Tilemap _grassTilemap;
    private Tilemap _groundTilemap;
    private Tilemap _flowersTilemap;

    private Vector3[] cowStartPositions;



    private bool _isGameOver = false;
    [SerializeField] private float timeToCheck = 0.35f;
    private float _moveCountdown;
    private bool _won;

    private Dictionary<Vector3Int, TileBase> _initialGrassTiles = new Dictionary<Vector3Int, TileBase>();
    private Dictionary<Vector3Int, TileBase> _initialGroundTiles = new Dictionary<Vector3Int, TileBase>();

    private void Awake()
    {
        _grassTilemap = grid.transform.Find("Grass").GetComponent<Tilemap>();
        _groundTilemap = grid.transform.Find("Ground").GetComponent<Tilemap>();
        _flowersTilemap = grid.transform.Find("Flowers").GetComponent<Tilemap>();
        CacheInitialTilemaps();
    }

    private void CacheInitialTilemaps()
    {
        _initialGrassTiles.Clear();
        _initialGroundTiles.Clear();

        foreach (var pos in _grassTilemap.cellBounds.allPositionsWithin)
        {
            if (_grassTilemap.HasTile(pos))
                _initialGrassTiles[pos] = _grassTilemap.GetTile(pos);
        }

        foreach (var pos in _groundTilemap.cellBounds.allPositionsWithin)
        {
            if (_groundTilemap.HasTile(pos))
                _initialGroundTiles[pos] = _groundTilemap.GetTile(pos);
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
        BoundsInt bounds = _groundTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            totalCount++;
            if (_grassTilemap.HasTile(pos))
                grassCount++;
        }

        if (totalCount > 0)
        {
            float coverage = (float)grassCount / totalCount;
            percentageText.text = $"{coverage * 100:0}%";
            _won = coverage >= threshold;
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
        _grassTilemap.ClearAllTiles();
        _groundTilemap.ClearAllTiles();
        _flowersTilemap.ClearAllTiles();

        foreach (var tile in _initialGroundTiles)
            _groundTilemap.SetTile(tile.Key, tile.Value);

        foreach (var tile in _initialGrassTiles)
            _grassTilemap.SetTile(tile.Key, tile.Value);
    }
}
