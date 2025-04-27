using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitFactory : SerializedSingleton<UnitFactory>, IUnitFactory
{
    [TabGroup("Unit Factory")][Tooltip("UnitData SO ¡ê Prefab ¸ÅÇÎ")][SerializeField] private List<UnitPrefabEntry> _entries = new List<UnitPrefabEntry>();

    private Dictionary<UnitBaseSO, GameObject> _map;
    private IPoolManager _pool;

    private void Start()
    {
        _pool = PoolManager.instance;
        _map = new Dictionary<UnitBaseSO, GameObject>(_entries.Count);
        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Data != null && e.Prefab != null) _map[e.Data] = e.Prefab;
        }
    }

    public UnitBase Create(UnitBaseSO data, Vector3 position)
    {
        if (!_map.TryGetValue(data, out var prefab)) return null;
        var go = _pool.Get(prefab);
        go.transform.position = position;

        var unit = go.GetComponent<UnitBase>();
        unit.UnitBaseData = data;
        unit.SetStatus();
        return unit;
    }

    public void Release(UnitBase unit)
    {
        if (unit == null) return;
        unit.gameObject.SetActive(false);
        _pool.Return(unit.gameObject);
    }
}
