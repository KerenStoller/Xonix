using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class Chicken : MonoBehaviour
{
    public static Chicken Instance {get; private set;}

    [SerializeField] private Collider2D chickenCollider;
    [SerializeField] private SpriteRenderer chickenSpriteRenderer;
    
    [SerializeField] private Grid grid;
    [SerializeField] private Movement chickenMovementScript;
    [SerializeField] private Tilemap flowerMap;
    [SerializeField] private Tile flowerTile;
    [SerializeField] private Tilemap grassMap;
    
    private bool justDied = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

        // Hide all flowers
        foreach (var pos in flowerPositions)
        {
            flowerMap.SetTile(pos, null);
        }
        yield return new WaitForSeconds(blinkInterval);

        // Show all flowers
        foreach (var pos in flowerPositions)
        {
            flowerMap.SetTile(pos, flowerTile);
        }

        yield return new WaitForSeconds(blinkInterval);
        
        // Hide all flowers again
        foreach (var pos in flowerPositions)
        {
            flowerMap.SetTile(pos, null);
        }
        yield return new WaitForSeconds(blinkInterval);
        
        chickenMovementScript.ResetPosition();
        ShowChicken();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flower"))
        {
            Debug.Log("hit flower");
            Debug.Log("flower position: " + transform.position);
            justDied = true;
            chickenMovementScript.StopMovement();
            StartCoroutine(BlinkFlowersAndRemove(0.2f));
        }
        if (other.gameObject.CompareTag("Grass"))
        {
            Debug.Log("TOUCHED GRASS");
            Debug.Log("justDied: " + justDied);
            Debug.Log("grass position: " + transform.position);
            if (justDied)
            {
                Debug.Log("touches grass BUT JUST DIED - DO NOTHING");
                justDied = false;
            }
            else
            {
                Debug.Log("touched grass - STOP DRAWING FLOWERS");
                GrassFillScript.Instance.FillGrassFromFlowers(GetAllFlowerPositions());
            }
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("TOUCHED GROUND - START DRAWING FLOWERS");
        }
    }

    List<Vector3Int> GetAllFlowerPositions()
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
}
