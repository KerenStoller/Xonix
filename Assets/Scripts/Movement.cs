using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap grass;
    [SerializeField] private Vector3Int startingPosition;
    [SerializeField] private float timeToMove = 0.35f;
    [SerializeField] private bool isChicken;
    
    public Vector3? CurrentDirection;
    public bool hasDirection;
    private float _moveCountdown;
    
    private int _leftWallX;
    private int _rightWallX;
    private int _bottomWallY;
    private int _topWallY;

    void Start()
    {
        // Get game bounds
        transform.position = grid.GetCellCenterWorld(startingPosition);
        _leftWallX = grass.cellBounds.xMin;
        _rightWallX = grass.cellBounds.xMax - 1;  // xMax is exclusive, so subtract 1
        _bottomWallY = grass.cellBounds.yMin;
        _topWallY = grass.cellBounds.yMax - 1;    // yMax is exclusive, so subtract 1
    }

    // Update is called once per frame
    void Update()
    {
        _moveCountdown += Time.deltaTime;

        if (_moveCountdown >= timeToMove)
        {
            _moveCountdown = 0;
            Move();
        }
    }

    void Move()
    {
        if (hasDirection)
        {
            Vector3Int newPosition = Vector3Int.FloorToInt(transform.position + CurrentDirection!.Value);
            
            if(OutOfBounds(newPosition))
            {
                ResetMovement();
                return;
            }
            
            Vector3Int oldCellPosition = grid.WorldToCell(transform.position);
            transform.position = grid.GetCellCenterWorld(newPosition);
            
            if (isChicken)
            {
                Chicken.Instance.DrawFlower(oldCellPosition);
            }
        }
    }
    
    private bool OutOfBounds(Vector3Int position)
    {
        return position.x < _leftWallX || position.x > _rightWallX || position.y < _bottomWallY || position.y > _topWallY;
    }

    private void ResetMovement()
    {
        hasDirection = false;
        _moveCountdown = 0;
    }

    public void ResetPosition()
    {
        transform.position = grid.GetCellCenterWorld(startingPosition);
        transform.rotation = Quaternion.identity;
        ResetMovement();
    }
}
