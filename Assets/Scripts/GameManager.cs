using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Text percentageText;
    [SerializeField] private Text winText;
    
    
    [SerializeField] private float timeToCheck = 0.35f;
    private float _moveCountdown;
    private bool _won;
    
    // Update is called once per frame
    void Update()
    {
        if (!_won)
        {
            _moveCountdown += Time.deltaTime;

            if (_moveCountdown >= timeToCheck)
            {
                _moveCountdown = 0;
                if (!_won)
                {
                    IsGrassCoverageAboveThreshold();
                }
                if (_won)
                {
                    winText.gameObject.SetActive(true);
                }
            }   
        }
    }

    private void IsGrassCoverageAboveThreshold()
    {
        float threshold = 0.75f;
        int grassCount = 0;
        int totalCount = 0;
        BoundsInt bounds = grassTilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            totalCount++;
            if (grassTilemap.GetTile(pos))
            {
                grassCount++;
            }
        }

        float coverage = (float)grassCount / totalCount;
        percentageText.text = $"{coverage*100:0}%";

        _won = coverage > threshold;
    }
}
