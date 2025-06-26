using UnityEngine;

/// <summary>
/// bool 배열로 간단한 그리드 맵을 표현하는 클래스
/// </summary>
public class SimpleGridMap : MonoBehaviour, IGridMap
{
    [SerializeField] private int _width;         // 맵 가로 크기
    [SerializeField] private int _height;        // 맵 세로 크기
    [SerializeField] private float _cellSize = 1f;   // 한 칸 크기

    private bool[,] _walls;                      // 장애물 정보

    public int Width => _width;
    public int Height => _height;

    private void Awake()
    {
        _walls = new bool[_width, _height];
    }

    /// <summary>외부에서 벽 설정</summary>
    public void SetWall(int x, int y, bool wall)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height) return;
        _walls[x, y] = wall;
    }

    /// <summary>지정 좌표가 벽인지 반환</summary>
    public bool IsWall(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height) return true;
        return _walls[x, y];
    }

    /// <summary>월드 좌표를 그리드 좌표로 변환</summary>
    public Vector2Int WorldToGrid(Vector2 world)
    {
        int gx = Mathf.RoundToInt(world.x / _cellSize);
        int gy = Mathf.RoundToInt(world.y / _cellSize);
        return new Vector2Int(gx, gy);
    }

    /// <summary>그리드 좌표를 월드 좌표로 변환</summary>
    public Vector2 GridToWorld(Vector2Int grid)
    {
        return new Vector2(grid.x * _cellSize, grid.y * _cellSize);
    }
}
