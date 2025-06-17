    using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class ProjectileFactory : SerializedSingleton<ProjectileFactory>, IProjectileFactory
{
    [TabGroup("Projectile Factory")][Tooltip("ProjectileSO  Prefab  Ʈ")][SerializeField] private List<ProjectilePrefabEntry> _entries;

    private IPoolManager _pool;
    private Dictionary<ProjectileBaseSO, GameObject> _map;
    private Dictionary<WeaponType, ProjectileBaseSO> _typeToSO;

    private readonly List<ProjectileBase> _activeProjectiles = new List<ProjectileBase>();
    private bool _initialized;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (_initialized) return;

        _pool = PoolManager.instance;
        _map = new Dictionary<ProjectileBaseSO, GameObject>(_entries.Count);
        _typeToSO = new Dictionary<WeaponType, ProjectileBaseSO>(_entries.Count);

        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Data != null && e.Prefab != null)
            {
                _map[e.Data] = e.Prefab;
                _typeToSO[e.Data.WeaponType] = e.Data;
            }
        }

        _initialized = true;
    }

    public ProjectileBase Create(ProjectileBaseSO data, Vector3 position, Quaternion rotation)
    {
        if (!_initialized)
        {
            Init();
        }
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

    public ProjectileBase Create(ProjectileBaseSO data, Vector3 position, Quaternion rotation, WeaponBase weapon)
    {
        if (!_initialized)
        {
            Init();
        }
        if (!_map.TryGetValue(data, out var prefab)) return null;
        var go = _pool.Get(prefab);
        go.transform.position = position;
        go.transform.rotation = rotation;

        var projectile = go.GetComponent<ProjectileBase>();
        projectile.ProjectileBaseData = data;
        projectile.SetStatus(weapon);

        // 활성 유닛으로 등록
        _activeProjectiles.Add(projectile);

        return projectile;
    }

    public ProjectileBaseSO GetProjectileData(WeaponType type)
    {
        _typeToSO.TryGetValue(type, out var data);
        return data;
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
