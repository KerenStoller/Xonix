using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Vector3Int startingPosition;
    [SerializeField] private float timeToMove = 0.35f;
    [SerializeField] private bool isChicken;
    
    public Vector3? CurrentDirection;
    public bool hasDirection;
    private float _moveCountdown;

    void Start()
    {
        transform.position = grid.GetCellCenterWorld(startingPosition);
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
            
            if(Utils.OutOfBounds(newPosition))
            {
                StopMovement();
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
    
   

    public void StopMovement()
    {
        hasDirection = false;
        _moveCountdown = 0;
    }

    public void ResetPosition()
    {
        transform.position = grid.GetCellCenterWorld(startingPosition);
        transform.rotation = Quaternion.identity;
    }
}
