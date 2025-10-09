using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class Chicken : MonoBehaviour
{
    public static Chicken Instance { get; private set; }
    
    [SerializeField] private Collider2D chickenCollider;
    [SerializeField] private SpriteRenderer chickenSpriteRenderer;
    [SerializeField] private Grid grid;
    [SerializeField] private Movement chickenMovementScript;
    [SerializeField] private Tilemap flowerMap;
    [SerializeField] private Tile flowerTile;
    [SerializeField] private Tilemap grassMap;
    [SerializeField] private Image[] heartImages;
    
    public int maxHealth = 3;
    private int currentHealth;
    private bool _justDied;

    public static event Action OnPlayerDied;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateHealthUI();
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
        if (!grassMap.HasTile(cellPosition))
        {
            flowerMap.SetTile(cellPosition, flowerTile);
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
            flowerMap.SetTile(pos, null);
        }
        yield return new WaitForSeconds(blinkInterval);

        foreach (var pos in flowerPositions)
        {
            flowerMap.SetTile(pos, flowerTile);
        }
        yield return new WaitForSeconds(blinkInterval);

        foreach (var pos in flowerPositions)
        {
            flowerMap.SetTile(pos, null);
        }
        yield return new WaitForSeconds(blinkInterval);

        flowerMap.ClearAllTiles();

        chickenMovementScript.ResetPosition();
        ShowChicken();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cow") || other.gameObject.CompareTag("Flower"))
        {
            _justDied = true;
            currentHealth--;
            UpdateHealthUI();
            
            if (currentHealth <= 0)
            {
                OnPlayerDied?.Invoke();
            }

            chickenMovementScript.StopMovement();
            StartCoroutine(BlinkFlowersAndRemove(0.2f));
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
        foreach (var pos in flowerMap.cellBounds.allPositionsWithin)
        {
            if (flowerMap.HasTile(pos))
            {
                flowerPositions.Add(pos);
            }
        }
        return flowerPositions;
    }

    public void DieToCow()
    {
        currentHealth = 0;
        UpdateHealthUI();
        OnPlayerDied?.Invoke();
        chickenMovementScript.StopMovement();
        StartCoroutine(BlinkFlowersAndRemove(0.2f));
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentHealth);
        }
    }
}
