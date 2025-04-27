using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class ProjectileFactory : SerializedSingleton<ProjectileFactory>, IProjectileFactory
{
    [TabGroup("Projectile Factory")][Tooltip("ProjectileSO → Prefab 매핑 리스트")][SerializeField] private List<ProjectilePrefabEntry> _entries;

    private Dictionary<ProjectileBaseSO, GameObject> _map;
    private IPoolManager _pool;

    // 활성화된(생성된) 발사체 목록
    private readonly List<ProjectileBase> _activeProjectiles = new List<ProjectileBase>();

    private void Start()
    {
        _pool = PoolManager.instance;
        _map = new Dictionary<ProjectileBaseSO, GameObject>(_entries.Count);

        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Data != null && e.Prefab != null) _map[e.Data] = e.Prefab;
        }
    }

    public ProjectileBase Create(ProjectileBaseSO data, Vector3 position, Quaternion rotation)
    {
        if (!_map.TryGetValue(data, out var prefab)) return null;
        var go = _pool.Get(prefab);
        go.transform.position = position;   
        go.transform.rotation = rotation;

        var projectile = go.GetComponent<ProjectileBase>();
        projectile.ProjectileBaseData = data;
        projectile.SetStatus();

        // 활성 유닛으로 등록
        _activeProjectiles.Add(projectile);

        return projectile;
    }

    public void Release(ProjectileBase projectile)
    {
        if (projectile == null) return;

        // 활성 목록에서 제거
        if (_activeProjectiles.Contains(projectile))
        {
            _activeProjectiles.Remove(projectile);
        }

        projectile.gameObject.SetActive(false);
        _pool.Return(projectile.gameObject);
    }

    public void ReleaseAll()
    {
        for (int i = _activeProjectiles.Count - 1; i >= 0; i--)
        {
            var unit = _activeProjectiles[i];
            if (unit != null)
            {
                unit.gameObject.SetActive(false);
                _pool.Return(unit.gameObject);
            }
        }
        _activeProjectiles.Clear();
    }

}
