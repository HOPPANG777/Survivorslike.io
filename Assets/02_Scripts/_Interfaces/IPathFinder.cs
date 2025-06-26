using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 경로 탐색 기능을 제공하는 인터페이스
/// </summary>
public interface IPathFinder
{
    /// <summary>시작 위치에서 목표 위치까지 경로를 계산한다</summary>
    List<Vector2> FindPath(Vector2 start, Vector2 goal);
}
