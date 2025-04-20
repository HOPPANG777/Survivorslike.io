using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [TabGroup("Collision Controller")][Tooltip("격자 셀 크기(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int cellSize;
    public float CellSize { get { return cellSize / 100.0f; } }

    [TabGroup("Collision Controller")][SerializeField] private SpatialHashCollisionController _controller;
    [TabGroup("Collision Controller")][SerializeField] private List<ICollidable> _collidables = new List<ICollidable>(); // 이제 이 리스트만 순회합니다.
    [TabGroup("Collision Controller")][SerializeField] private List<ICollision> _responders = new List<ICollision>();

    void Awake()
    {
        _controller = new SpatialHashCollisionController(cellSize);

        _responders.Add(new HittedController(10f));
        _responders.Add(new PushController());
    }

    /// <summary>Collidable 등록</summary>
    public void RegisterCollidable(ICollidable c)
    {
        if (!_collidables.Contains(c)) _collidables.Add(c);
    }

    /// <summary>Collidable 해제</summary>
    public void UnregisterCollidable(ICollidable c)
    {
        _collidables.Remove(c);
    }

    public List<ICollidable> GetAllCollidables() => _collidables;

    void Update()
    {
        // 1) 충돌 검출 대상: 미리 등록된 리스트만 사용
        var entities = _collidables;

        // 2) 격자 해시 + 제곱거리 narrow‑phase 로 충돌 쌍 뽑기
        //    .ToList() 로 배열화해서 for 루프 돌립니다.
        var collisionPairs = _controller.Detect(entities).ToList();

        int pairCount = collisionPairs.Count;
        for (int pi = 0; pi < pairCount; pi++)
        {
            var a = collisionPairs[pi].A;
            var b = collisionPairs[pi].B;

            // 3) 등록된 모든 리스폰더에 통보 (양방향)
            int respCount = _responders.Count;
            for (int ri = 0; ri < respCount; ri++)
            {
                var responder = _responders[ri];
                responder.OnCollision(a, b);
                responder.OnCollision(b, a);
            }
        }
    }
}
