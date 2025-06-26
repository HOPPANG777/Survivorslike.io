using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* 알고리즘을 사용하여 경로를 계산하는 클래스
/// </summary>
public class AStarPathFinder : IPathFinder
{
    /// <summary>맵 정보 제공자</summary>
    private readonly IGridMap _map;

    /// <summary>
    /// 내부에서 사용할 노드 정보
    /// </summary>
    private class Node
    {
        public Vector2Int Pos;   // 현재 좌표
        public Node Parent;      // 부모 노드
        public int G;            // 시작점에서의 실제 비용
        public int H;            // 휴리스틱 값
        public int F => G + H;   // 총 비용
    }

    /// <summary>
    /// 생성자에서 맵 정보를 주입 받음
    /// </summary>
    public AStarPathFinder(IGridMap map)
    {
        _map = map;
    }

    /// <summary>
    /// 맨해튼 거리 휴리스틱 계산
    /// </summary>
    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    /// <summary>
    /// 실제 경로 계산 메서드
    /// </summary>
    public List<Vector2> FindPath(Vector2 startWorld, Vector2 goalWorld)
    {
        Vector2Int start = _map.WorldToGrid(startWorld);
        Vector2Int goal = _map.WorldToGrid(goalWorld);

        List<Node> open = new List<Node>();            // 열린 리스트
        bool[,] closed = new bool[_map.Width, _map.Height];
        Node[,] nodes = new Node[_map.Width, _map.Height];

        Node startNode = new Node { Pos = start, G = 0, H = Heuristic(start, goal) };
        nodes[start.x, start.y] = startNode;
        open.Add(startNode);

        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        while (open.Count > 0)
        {
            // f 값이 가장 작은 노드 선택
            int index = 0;
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].F < open[index].F) index = i;
            }
            Node current = open[index];
            open.RemoveAt(index);

            if (closed[current.Pos.x, current.Pos.y]) continue;
            closed[current.Pos.x, current.Pos.y] = true;

            // 목표 도착 시 경로 복원
            if (current.Pos == goal)
            {
                List<Vector2> path = new List<Vector2>();
                Node node = current;
                while (node != null)
                {
                    path.Add(_map.GridToWorld(node.Pos));
                    node = node.Parent;
                }
                path.Reverse();
                return path;
            }

            for (int dir = 0; dir < 4; dir++)
            {
                int nx = current.Pos.x + dx[dir];
                int ny = current.Pos.y + dy[dir];

                if (_map.IsWall(nx, ny)) continue;
                if (closed[nx, ny]) continue;

                int g = current.G + 1;

                Node neighbor = nodes[nx, ny];
                if (neighbor == null)
                {
                    neighbor = new Node();
                    neighbor.Pos = new Vector2Int(nx, ny);
                    nodes[nx, ny] = neighbor;
                }

                if (g < neighbor.G || neighbor.Parent == null)
                {
                    neighbor.G = g;
                    neighbor.H = Heuristic(neighbor.Pos, goal);
                    neighbor.Parent = current;

                    bool exists = false;
                    for (int i = 0; i < open.Count; i++)
                    {
                        if (open[i].Pos == neighbor.Pos)
                        {
                            open[i] = neighbor;
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) open.Add(neighbor);
                }
            }
        }
        return null;    // 경로를 찾지 못한 경우
    }
}
