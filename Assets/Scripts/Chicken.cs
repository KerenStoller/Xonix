using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class Chicken : MonoBehaviour
{
    public static Chicken Instance { get; private set; }

    [SerializeField] private Collider2D chickenCollider;
    [SerializeField] private SpriteRenderer chickenSpriteRenderer;
    [SerializeField] private Grid grid;
    [SerializeField] private Movement chickenMovementScript;
    [SerializeField] private Tile flowerTile;
    
    private Tilemap _grassTilemap;
    private Tilemap _flowersTilemap;
    private bool _justDied;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _grassTilemap = grid.transform.Find("Grass").GetComponent<Tilemap>();
        _flowersTilemap = grid.transform.Find("Flowers").GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            chickenMovementScript.CurrentDirection = Vector3.right;
            chickenMovementScript.hasDirection = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            chickenMovementScript.CurrentDirection = Vector3.left;
            chickenMovementScript.hasDirection = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            chickenMovementScript.CurrentDirection = Vector3.down;
            chickenMovementScript.hasDirection = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            chickenMovementScript.CurrentDirection = Vector3.up;
            chickenMovementScript.hasDirection = true;
        }
    }

    public void DrawFlower(Vector3Int cellPosition)
    {
        if (!_grassTilemap.HasTile(cellPosition))
        {
            _flowersTilemap.SetTile(cellPosition, flowerTile);
        }
    }

    private void HideChicken()
    {
        chickenCollider.enabled = false;
        chickenSpriteRenderer.enabled = false;
    }

    private void ShowChicken()
    {
        chickenCollider.enabled = true;
        chickenSpriteRenderer.enabled = true;
    }

    private IEnumerator BlinkFlowersAndRemove(float blinkInterval)
    {
        HideChicken();
        List<Vector3Int> flowerPositions = GetAllFlowerPositions();

        foreach (var pos in flowerPositions)
        {
            _flowersTilemap.SetTile(pos, null);
        }
        yield return new WaitForSeconds(blinkInterval);

        foreach (var pos in flowerPositions)
        {
            _flowersTilemap.SetTile(pos, flowerTile);
        }
        yield return new WaitForSeconds(blinkInterval);

        foreach (var pos in flowerPositions)
        {
            _flowersTilemap.SetTile(pos, null);
        }
        yield return new WaitForSeconds(blinkInterval);

        _flowersTilemap.ClearAllTiles();

        chickenMovementScript.ResetPosition();
        ShowChicken();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cow") || other.gameObject.CompareTag("Flower"))
        {
            _justDied = true;
            GameManager.Instance.RemoveLife();
            chickenMovementScript.StopMovement();
            StartCoroutine(BlinkFlowersAndRemove(0.2f));
            _justDied = false;
            return;
        }

        if (other.gameObject.CompareTag("Grass"))
        {
            if (_justDied)
            {
                _justDied = false;
            }
            else
            {
                GrassFillScript.Instance.FillGrassFromFlowers(GetAllFlowerPositions());
            }
        }
    }

    private List<Vector3Int> GetAllFlowerPositions()
    {
        List<Vector3Int> flowerPositions = new List<Vector3Int>();
        foreach (var pos in _flowersTilemap.cellBounds.allPositionsWithin)
        {
            if (_flowersTilemap.HasTile(pos))
            {
                flowerPositions.Add(pos);
            }
        }
        return flowerPositions;
    }

    public void DieFromCow() 
    {
        if (_justDied) return; 
        _justDied = true;
        GameManager.Instance.RemoveLife();
        chickenMovementScript.StopMovement();
        StartCoroutine(BlinkFlowersAndRemove(0.2f));
    }
}