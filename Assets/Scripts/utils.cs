using UnityEngine;
using UnityEngine.Tilemaps;

public static class Utils
{
    private static int _leftWallX;
    private static int _rightWallX;
    private static int _bottomWallY;
    private static int _topWallY;
    
    public static void InitializeBounds(Tilemap grass)
    {
        // Get game bounds from grass tilemap
        _leftWallX = grass.cellBounds.xMin;
        _rightWallX = grass.cellBounds.xMax - 1; // xMax is exclusive, so subtract 1
        _bottomWallY = grass.cellBounds.yMin;
        _topWallY = grass.cellBounds.yMax - 1;   // yMax is exclusive, so subtract 1
    }
    
    public static bool OutOfBounds(Vector3Int position)
    {
        return position.x < _leftWallX || position.x > _rightWallX || position.y < _bottomWallY || position.y > _topWallY;
    }
}