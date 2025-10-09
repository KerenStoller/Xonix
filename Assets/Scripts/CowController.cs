using UnityEngine;
using UnityEngine.Tilemaps;

public class CowController : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap grassTilemap; // Added this field
    [SerializeField] private Tilemap flowerTilemap;
    [SerializeField] private float moveInterval = 0.35f;
    private float moveCountdown;
    private SpriteRenderer spriteRenderer;


    void Update()
    {
        moveCountdown += Time.deltaTime;
        if (moveCountdown >= moveInterval)
        {
            moveCountdown = 0;
            MoveRandom();
        }
    }

void Awake()
{
    spriteRenderer = GetComponent<SpriteRenderer>();
}

void MoveRandom()
{
    Vector3Int[] directions = new Vector3Int[]
    {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
    };

    directions = ShuffleDirections(directions);

    foreach (var dir in directions)
    {
        Vector3Int currentCell = grid.WorldToCell(transform.position);
        Vector3Int nextCell = currentCell + dir;

        if (!Utils.OutOfBounds(nextCell)
            && groundTilemap.HasTile(nextCell)
            && !grassTilemap.HasTile(nextCell))
        {
            // Flip cow sprite if moving left/right
            if (dir == Vector3Int.left)
                spriteRenderer.flipX = true;
            else if (dir == Vector3Int.right)
                spriteRenderer.flipX = false;

            transform.position = grid.GetCellCenterWorld(nextCell);
            break;
        }
    }

    // Check for cow-chicken collision
    Vector3Int cowCell = grid.WorldToCell(transform.position);
    Vector3Int chickenCell = grid.WorldToCell(Chicken.Instance.transform.position);

    if (cowCell == chickenCell)
    {
        Chicken.Instance.DieToCow();
    }

    // Kill chicken if cow steps onto flower
    if (flowerTilemap.HasTile(cowCell))
    {
        Chicken.Instance.DieToCow();
    }
}

    Vector3Int[] ShuffleDirections(Vector3Int[] dirs)
    {
        for (int i = dirs.Length - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            var temp = dirs[i];
            dirs[i] = dirs[rand];
            dirs[rand] = temp;
        }
        return dirs;
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Flower"))
    {
        Debug.Log("Cow hit flower!");
        // Find chicken and kill it (direct reference or via GameManager)
        Chicken chicken = FindAnyObjectByType<Chicken>(); // (or get reference some other way)
        if (chicken != null)
        {
            chicken.DieToCow(); // call the same public death/reset method
        }
    }
}

}
