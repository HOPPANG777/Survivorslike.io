using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponFactory : SerializedSingleton<WeaponFactory>, IWeaponFactory
{
    [TabGroup("Unit Factory")][Tooltip("UnitData SO ↔ Prefab 매핑")][SerializeField] private List<WeaponPrefabEntry> _entries = new List<WeaponPrefabEntry>();

    private IPoolManager _pool;
    private Dictionary<WeaponBaseSO, GameObject> _map;
    private Dictionary<WeaponType, WeaponBaseSO> _typeToSO;

    // 활성화된(생성된) 유닛 목록
    private readonly List<WeaponBase> _activeWeapons = new List<WeaponBase>();

    private void Start()
    {
        _pool = PoolManager.instance;
        _map = new Dictionary<WeaponBaseSO, GameObject>(_entries.Count);
        _typeToSO = new Dictionary<WeaponType, WeaponBaseSO>(_entries.Count);

        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Data != null && e.Prefab != null)
            {
                _map[e.Data] = e.Prefab;
                _typeToSO[e.Data.WeaponType] = e.Data; // SO에 WeaponType 프로퍼티가 있음
            }
        }
    }

    //public WeaponBase Create(WeaponBaseSO data, Vector3 position, Quaternion rotation)
    //{
    //    if (!_map.TryGetValue(data, out var prefab)) return null;
    //    var go = _pool.Get(prefab);
    //    go.transform.position = position;
    //    go.transform.rotation = rotation;

    //    var weapon = go.GetComponent<WeaponBase>();
    //    weapon.WeaponBaseData = data;
    //    weapon.SetStatus();

    //    // 활성 유닛으로 등록
    //    _activeWeapons.Add(weapon);

    //    return weapon;
    //}

    public WeaponBase Create(WeaponType type, Transform parent = null)
    {
        if (!_typeToSO.TryGetValue(type, out var data)) return null;
        var go = _pool.Get(_map[data]);

        if (parent != null) go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        var weapon = go.GetComponent<WeaponBase>();
        weapon.WeaponBaseData = data;
        weapon.SetStatus();

        // 활성 유닛으로 등록
        _activeWeapons.Add(weapon);
        
        return weapon;
    }

    public void Release(WeaponBase weapon)
    {
        if (weapon == null) return;

        // 활성 목록에서 제거
        if (_activeWeapons.Contains(weapon))
        {
            _activeWeapons.Remove(weapon);
        }

        weapon.gameObject.SetActive(false);
        _pool.Return(weapon.gameObject);
    }

    public void ReleaseAll()
    {
        for (int i = _activeWeapons.Count - 1; i >= 0; i--)
        {
            var unit = _activeWeapons[i];
            if (unit != null)
            {
                unit.gameObject.SetActive(false);
                _pool.Return(unit.gameObject);
            }
        }
        _activeWeapons.Clear();
    }
}

