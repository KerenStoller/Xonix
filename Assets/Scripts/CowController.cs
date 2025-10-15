using UnityEngine;
using Random = UnityEngine.Random;

public class CowController : MonoBehaviour
{
    [SerializeField] private Movement cowMovementScript;
    [SerializeField] private float randomMoveInterval = 2f;
    private float _moveCountdown;

    private readonly Vector3Int[] _directions =
    {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
    };

    private void Start()
    {
        cowMovementScript.CurrentDirection = _directions[Random.Range(0, _directions.Length)];
        cowMovementScript.hasDirection = true;
    }
    
    private void Update()
    {
        if (!cowMovementScript.hasDirection)
        {
            var randomizedDirections = GetRandomizedDirections();
            foreach (var dir in randomizedDirections)
            {
                cowMovementScript.CurrentDirection = dir;
                cowMovementScript.hasDirection = true;
                cowMovementScript.Move();
                if (cowMovementScript.hasDirection)
                {
                    break;
                }
            }
            // If no valid direction found, do nothing this frame
            return;
        }
        
        _moveCountdown += Time.deltaTime;
        if (_moveCountdown >= randomMoveInterval)
        {
            _moveCountdown = 0;
            MoveRandom();
        }
    }

    private Vector3Int[] GetRandomizedDirections()
    {
        var directions = (Vector3Int[])_directions.Clone();
        for (int i = directions.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (directions[i], directions[rnd]) = (directions[rnd], directions[i]);
        }
        return directions;
    }

    void MoveRandom()
    {
        cowMovementScript.CurrentDirection = _directions[Random.Range(0, _directions.Length)];
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flower"))
        {
            Chicken.Instance.DieFromCow();
        }
    }
}
