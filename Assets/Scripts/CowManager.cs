using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CowManager : MonoBehaviour
{
    public static CowManager Instance { get; private set; }
    
    [SerializeField] private int numberOfCows = 3;
    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private Sprite[] sprites;

    public List<GameObject> cows = new List<GameObject>();
    
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        for (int i = 0; i < numberOfCows; i++)
        {
            cowPrefab = Instantiate(cowPrefab, transform);
            var cowMovement = cowPrefab.GetComponent<Movement>();
            cowMovement.startingPosition = new Vector3Int(
                Random.Range(-5, 5),
                Random.Range(-5, 5), 0
            );
            cowMovement.grid = grid;
            cowMovement.sprites = sprites;
            cows.Add(cowPrefab);
        }
    }
}
