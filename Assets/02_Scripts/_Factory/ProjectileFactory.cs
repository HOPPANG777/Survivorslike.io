using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ProjectileFactory : SerializedSingleton<ProjectileFactory>, IProjectileFactory
{
    [TabGroup("Projectile Factory")][Tooltip("ProjectileSO → Prefab 매핑 리스트")][SerializeField] private List<ProjectilePrefabEntry> _entries;

    private Dictionary<ProjectileBaseSO, GameObject> _map;
    private IPoolManager _pool;

    private void Start()
    {
        _pool = PoolManager.Instance;
        _map = new Dictionary<ProjectileBaseSO, GameObject>(_entries.Count);

        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Data != null && e.Prefab != null) _map[e.Data] = e.Prefab;
        }
    }

    public GameObject Create(ProjectileBaseSO data, Vector3 position, Quaternion rotation)
    {
        if (!_map.TryGetValue(data, out var prefab))
        {
            Debug.LogError($"ProjectileFactory: 매핑된 Prefab이 없습니다! Data = {data.name}");
            return null;
        }

        var go = _pool.Get(prefab);
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }

    public void Release(GameObject projectile)
    {
        if (projectile == null)
        {
            return;
        }

        projectile.SetActive(false);
        _pool.Return(projectile);
    }
}
