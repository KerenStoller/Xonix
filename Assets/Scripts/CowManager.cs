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
            GameObject cow = CowPool.Instance.GetCow();
            cow.transform.SetParent(transform); // Assign parent for hierarchy organization
            // Set up cow's starting properties
            var cowMovement = cow.GetComponent<Movement>();
            cowMovement.startingPosition = new Vector3Int(
                Random.Range(-5, 5),
                Random.Range(-5, 5), 0
            );
            cowMovement.grid = grid;
            cowMovement.sprites = sprites;
            cow.SetActive(true); // Make sure it's enabled when retrieved
            cows.Add(cow);
        }

    }
    
    public void ReturnAllCowsToPool()
{
    foreach (var cow in cows)
    {
        CowPool.Instance.ReturnCow(cow);
    }
    cows.Clear();
}

}
