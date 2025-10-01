using UnityEngine;
using UnityEngine.Tilemaps;

public class Chicken : MonoBehaviour
{
    public static Chicken Instance {get; private set;}
    
    [SerializeField] private Grid grid;
    [SerializeField] private Movement chickenMovementScript;
    [SerializeField] private Tilemap flowerMap;
    [SerializeField] private Tile flowerTile;
    [SerializeField] private Tilemap grassMap;
    
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
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Flower"))
        {
            chickenMovementScript.ResetPosition();
            flowerMap.ClearAllTiles();
            flowerMap.RefreshAllTiles();
        }
        // if collision is a cow
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Grass"))
        {
            Debug.Log("TOUCHED GRASS - STOP DRAWING FLOWERS");
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("TOUCHED GROUND - START DRAWING FLOWERS");
        }
    }
}
