using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    [SerializeField] public Grid grid;
    [SerializeField] public Vector3Int startingPosition;
    [SerializeField] private float timeToMove = 0.35f;
    [SerializeField] private bool isChicken;
    [SerializeField] public Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Tilemap _grassTilemap;
    private int _currentSpriteIndex;
    public Vector3? CurrentDirection;
    public bool hasDirection;
    private float _moveCountdown;

    private void Awake()
    {
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        _grassTilemap = grid.transform.Find("Grass").GetComponent<Tilemap>();
        transform.position = grid.GetCellCenterWorld(startingPosition);
    }
    
    void Update()
    {
        _moveCountdown += Time.deltaTime;

        if (_moveCountdown >= timeToMove)
        {
            _moveCountdown = 0;
            Move();
        }
    }

    public void Move()
    {
        if (hasDirection)
        {
            Vector3Int newPosition = Vector3Int.FloorToInt(transform.position + CurrentDirection!.Value);

            if (!isChicken)
            {
                if (_grassTilemap.HasTile(newPosition))
                {
                    StopMovement();
                    return;
                }
            }
            
            if(Utils.OutOfBounds(newPosition))
            {
                StopMovement();
                return;
            }
            
            Vector3Int oldCellPosition = grid.WorldToCell(transform.position);
            transform.position = grid.GetCellCenterWorld(newPosition);
            spriteRenderer.sprite = sprites[++_currentSpriteIndex % sprites.Length];
            if (CurrentDirection!.Value == Vector3Int.left)
            {
                spriteRenderer.flipX = true;
            }
            else if (CurrentDirection!.Value == Vector3Int.right)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.sprite = sprites[0];
            }
                
            if (isChicken)
            {
                Chicken.Instance.DrawFlower(oldCellPosition);
            }
        }
    }

    public void StopMovement()
    {
        hasDirection = false;
        _moveCountdown = 0;
    }

    public void ResetPosition()
    {
        transform.position = grid.GetCellCenterWorld(startingPosition);
        transform.rotation = Quaternion.identity;
        spriteRenderer.flipX = false;
    }
}
