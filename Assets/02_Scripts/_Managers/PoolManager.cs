using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : SerializedSingleton<PoolManager>, IPoolManager
{
    [TabGroup("PoolManager")][Tooltip("풀 설정 리스트")][SerializeField] private List<PoolConfig> _configs;

    // Prefab -> 인스턴스 큐
    private Dictionary<GameObject, Queue<GameObject>> _pools;

    // Prefab -> 생성된 인스턴스의 부모 트랜스폼
    private Dictionary<GameObject, Transform> _poolParents;

    // Prefab -> 현재까지 생성된 인스턴스 수
    private Dictionary<GameObject, int> _poolInstanceCount;

    // 인스턴스 -> Prefab 매핑
    private Dictionary<GameObject, GameObject> _instanceToPrefab;

    private bool _initialized;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        if (_initialized) return;

        _pools = new Dictionary<GameObject, Queue<GameObject>>(_configs.Count);
        _poolParents = new Dictionary<GameObject, Transform>(_configs.Count);
        _poolInstanceCount = new Dictionary<GameObject, int>(_configs.Count);
        _instanceToPrefab = new Dictionary<GameObject, GameObject>(_configs.Count * 2);

        // 각 프리팹별 큐 초기화
        for (int i = 0; i < _configs.Count; i++)
        {
            var prefab = _configs[i].Prefab;
            var queue = new Queue<GameObject>(_configs[i].InitialSize);

            // 프리팹 이름 + "Pool" 오브젝트 생성
            var parent = new GameObject(prefab.name + "Pool").transform;
            parent.SetParent(transform);
            _poolParents[prefab] = parent;

            int count = 0;
            for (int j = 0; j < _configs[i].InitialSize; j++)
            {
                count++;
                var go = Instantiate(prefab, parent);
                go.name = $"{prefab.name}_{count:00}";
                go.SetActive(false);
                queue.Enqueue(go);
                _instanceToPrefab[go] = prefab;
            }
            _pools[prefab] = queue;
            _poolInstanceCount[prefab] = count;
        }

        _initialized = true;
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

        // 필요한 데이터가 없으면 초기화
        if (!_pools.ContainsKey(prefab))
        {
            _pools[prefab] = new Queue<GameObject>();
        }

        if (!_poolParents.TryGetValue(prefab, out var parent))
        {
            parent = new GameObject(prefab.name + "Pool").transform;
            parent.SetParent(transform);
            _poolParents[prefab] = parent;
            _poolInstanceCount[prefab] = 0;
        }

        int count = _poolInstanceCount[prefab] + 1;
        _poolInstanceCount[prefab] = count;

        var inst = Instantiate(prefab, parent);
        inst.name = $"{prefab.name}_{count:00}";
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
        if (!_pools.TryGetValue(prefab, out var queue))
        {
            queue = new Queue<GameObject>();
            _pools[prefab] = queue;
        }

        if (_poolParents.TryGetValue(prefab, out var parent))
        {
            instance.transform.SetParent(parent);
        }
        instance.SetActive(false);
        queue.Enqueue(instance);
    }

    // 인터페이스 상속 구현 (프리팹 없이 직접 Return 호출)
    void IObjectPool.Return(GameObject instance) => Return(instance);
}
