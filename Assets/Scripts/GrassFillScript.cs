using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GrassFillScript : MonoBehaviour
{
    public static GrassFillScript Instance { get; private set; }
    
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tile grassTile;
    [SerializeField] private Tilemap flowerTilemap;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private List<Vector3> cowsWorldPositions;
    
    HashSet<Vector3Int> _cowPositions = new HashSet<Vector3Int>();

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Utils.InitializeBounds(grassTilemap);
        
        foreach (var worldPos in cowsWorldPositions)
        {
            Vector3Int cellPos = grassTilemap.WorldToCell(worldPos);
            _cowPositions.Add(cellPos);
        }
        
        print(_cowPositions.Count);
        print(_cowPositions);
    }
    
    public void FillGrassFromFlowers(List<Vector3Int> flowerCellPositions)
    {
        flowerTilemap.ClearAllTiles();
        ColorListToGrass(flowerCellPositions);
        foreach (Vector3Int flowerCellPosition in flowerCellPositions)
        {
            FillGrassFromFlower(flowerCellPosition);
        }
    }

    private void FillGrassFromFlower(Vector3Int flowerCellPosition)
    {
        UpdateCowPositions();
        List<Vector3Int> potentialGrass = new List<Vector3Int>();
        Vector3Int[] neighbors = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        for (int i = 0; i < neighbors.Length; i++)
        {
            neighbors[i] += flowerCellPosition;
        }

        foreach (Vector3Int adjacentCell in neighbors)
        {
            if (!Utils.OutOfBounds(adjacentCell))
            {
                if (groundTilemap.HasTile(adjacentCell) &&
                    !grassTilemap.HasTile(adjacentCell))
                {
                    potentialGrass.Clear();
                    potentialGrass.Add(adjacentCell);
                    if (!FindCows(adjacentCell, potentialGrass))
                    {
                        ColorListToGrass(potentialGrass);
                    }
                }
            }
        }
    }

    private bool FindCows(Vector3Int groundCellPosition, List<Vector3Int> potentialGrass)
    {
        Vector3Int[] neighbors = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        for (int i = 0; i < neighbors.Length; i++)
        {
            neighbors[i] += groundCellPosition;
        }

        foreach (Vector3Int adjacentCell in neighbors)
        {
            if (!Utils.OutOfBounds(adjacentCell))
            {
                if (_cowPositions.Contains(adjacentCell))
                {
                    return true;
                }
                if (groundTilemap.HasTile(adjacentCell) &&
                    !potentialGrass.Contains(adjacentCell))
                {
                    potentialGrass.Add(adjacentCell);
                    if (FindCows(adjacentCell, potentialGrass))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void ColorListToGrass(List<Vector3Int> cellsToColor)
    {
        foreach (Vector3Int cell in cellsToColor)
        {
            groundTilemap.SetTile(cell, null);
            grassTilemap.SetTile(cell, grassTile);
        }
    }

    void UpdateCowPositions() 
    {
        _cowPositions.Clear();
        var cows = GameObject.FindGameObjectsWithTag("Cow");
        foreach (var cow in cows) 
        {
            Vector3Int cellPos = grassTilemap.WorldToCell(cow.transform.position);
            _cowPositions.Add(cellPos);
        }
    }

}
