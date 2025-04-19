using System.Collections.Generic;
using UnityEngine;

public class SpatialHashCollisionController : MonoBehaviour
{
    private readonly float _cellSize;
    private readonly Dictionary<Vector2Int, List<ICollidable>> _grid = new Dictionary<Vector2Int, List<ICollidable>>();

    public SpatialHashCollisionController(float cellSize)
    {
        _cellSize = cellSize;
    }

    /// <summary>
    /// 충돌 가능성 있는 쌍을 전부 찾아서 튜플로 반환.
    /// </summary>
    public IEnumerable<(ICollidable A, ICollidable B)> Detect(IEnumerable<ICollidable> entities)
    {
        _grid.Clear();

        // 1) 엔티티들을 격자(cell)에 분배
        foreach (var e in entities)
        {
            var key = ToCell(e.Position);
            if (!_grid.TryGetValue(key, out var list)) _grid[key] = list = new List<ICollidable>();
            list.Add(e);
        }

        float cellRange = _cellSize;
        // 2) 각 셀과 이웃 셀 내에서만 충돌 검사
        foreach (var kv in _grid)
        {
            var cell = kv.Key;
            var list = kv.Value;

            // (1) 같은 셀 내부
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (IsColliding(list[i], list[j])) yield return (list[i], list[j]);
                }
            }

            // (2) 8방향 이웃 셀
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var neighborKey = new Vector2Int(cell.x + dx, cell.y + dy);
                    if (!_grid.TryGetValue(neighborKey, out var list2)) continue;

                    foreach (var a in list)
                    {
                        foreach (var b in list2)
                        {
                            if (IsColliding(a, b)) yield return (a, b);
                        }
                    }
                }
            }
        }
    }

    private Vector2Int ToCell(Vector2 pos) => new Vector2Int(Mathf.FloorToInt(pos.x / _cellSize), Mathf.FloorToInt(pos.y / _cellSize));

    // 제곱 거리 비교로 narrow-phase
    private bool IsColliding(ICollidable a, ICollidable b)
    {
        float radii = a.CollisionRadius + b.CollisionRadius;
        float sqrRadii = radii * radii;
        return (b.Position - a.Position).sqrMagnitude < sqrRadii;
    }
}
