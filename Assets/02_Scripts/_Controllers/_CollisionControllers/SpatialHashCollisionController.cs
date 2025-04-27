using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpatialHashCollisionController
{
    private float _cellSize;
    private Dictionary<Vector2Int, List<ICollidable>> _grid = new Dictionary<Vector2Int, List<ICollidable>>();

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

        // 1) entities → 리스트로 변환 (인덱스로 접근하기 위함)
        var entityList = entities as IList<ICollidable> ?? entities.ToList();
        int entityCount = entityList.Count;

        // 2) 엔티티들을 격자(cell)에 분배
        for (int ei = 0; ei < entityCount; ei++)
        {
            var e = entityList[ei];
            var key = ToCell(e.Position);

            if (!_grid.TryGetValue(key, out var list))
            {
                list = new List<ICollidable>();
                _grid[key] = list;
            }
            list.Add(e);
        }

        // 3) 셀 키 목록을 리스트로 복사해서 인덱스로 순회
        var cellKeys = new List<Vector2Int>(_grid.Keys);
        int cellKeyCount = cellKeys.Count;

        for (int ki = 0; ki < cellKeyCount; ki++)
        {
            var cell = cellKeys[ki];
            var list = _grid[cell];
            int listCount = list.Count;

            // (1) 같은 셀 내부 충돌 검사
            for (int i = 0; i < listCount; i++)
            {
                for (int j = i + 1; j < listCount; j++)
                {
                    if (IsColliding(list[i], list[j])) yield return (list[i], list[j]);
                }
            }

            // (2) 8방향 이웃 셀 충돌 검사
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    var neighborKey = new Vector2Int(cell.x + dx, cell.y + dy);
                    if (!_grid.TryGetValue(neighborKey, out var list2)) continue;

                    int list2Count = list2.Count;
                    for (int ai = 0; ai < listCount; ai++)
                    {
                        for (int bi = 0; bi < list2Count; bi++)
                        {
                            if (IsColliding(list[ai], list2[bi])) yield return (list[ai], list2[bi]);
                        }
                    }
                }
            }
        }
    }

    private Vector2Int ToCell(Vector2 pos) => new Vector2Int( Mathf.FloorToInt(pos.x / _cellSize), Mathf.FloorToInt(pos.y / _cellSize));

    private bool IsColliding(ICollidable a, ICollidable b)
    {
        float radii = a.CollisionRadius + b.CollisionRadius;
        return (b.Position - a.Position).sqrMagnitude < radii * radii;
    }
}
