using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : SerializedSingleton<PoolManager>, IPoolManager
{
    [TabGroup("PoolManager")][Tooltip("풀 설정 리스트")][SerializeField] private List<PoolConfig> _configs;

    // Prefab -> 인스턴스 큐
    private Dictionary<GameObject, Queue<GameObject>> _pools;

    // 인스턴스 -> Prefab 매핑
    private Dictionary<GameObject, GameObject> _instanceToPrefab;

    public void Start()
    {
        _pools = new Dictionary<GameObject, Queue<GameObject>>(_configs.Count);
        _instanceToPrefab = new Dictionary<GameObject, GameObject>(_configs.Count * 2);

        // 각 프리팹별 큐 초기화
        for (int i = 0; i < _configs.Count; i++)
        {
            var prefab = _configs[i].Prefab;
            var queue = new Queue<GameObject>(_configs[i].InitialSize);
            for (int j = 0; j < _configs[i].InitialSize; j++)
            {
                var go = Instantiate(prefab, transform);
                go.SetActive(false);
                queue.Enqueue(go);
                _instanceToPrefab[go] = prefab;
            }
            _pools[prefab] = queue;
        }
    }

    // 풀에서 꺼내거나 새로 생성
    public GameObject Get(GameObject prefab)
    {
        if (_pools.TryGetValue(prefab, out var queue) && queue.Count > 0)
        {
            var go = queue.Dequeue();
            go.SetActive(true);
            return go;
        }
        var inst = Instantiate(prefab, transform);
        _instanceToPrefab[inst] = prefab;
        return inst;
    }

    // 인터페이스 상속 구현 (프리팹 없이 직접 Get 호출 불가)
    GameObject IObjectPool.Get() => null;

    // 반환
    public void Return(GameObject instance)
    {
        if (instance == null) return;
        if (!_instanceToPrefab.TryGetValue(instance, out var prefab))
        {
            Destroy(instance);
            return;
        }
        instance.SetActive(false);
        _pools[prefab].Enqueue(instance);
    }

    // 인터페이스 상속 구현 (프리팹 없이 직접 Return 호출)
    void IObjectPool.Return(GameObject instance) => Return(instance);
}
