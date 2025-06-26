using UnityEngine;

/// <summary>
/// 그리드 맵 정보를 제공하는 인터페이스
/// </summary>
public interface IGridMap
{
    /// <summary>맵 가로 크기</summary>
    int Width { get; }
    /// <summary>맵 세로 크기</summary>
    int Height { get; }
    /// <summary>특정 좌표가 벽인지 확인</summary>
    bool IsWall(int x, int y);
    /// <summary>월드 좌표를 그리드 좌표로 변환</summary>
    Vector2Int WorldToGrid(Vector2 world);
    /// <summary>그리드 좌표를 월드 좌표로 변환</summary>
    Vector2 GridToWorld(Vector2Int grid);
}
