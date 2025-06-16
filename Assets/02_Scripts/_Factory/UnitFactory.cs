using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitFactory : SerializedSingleton<UnitFactory>, IUnitFactory
{
    private IUIFactory _uiFactory;
        _uiFactory = UIFactory.instance;
        if (data is EnemyUnitSO) {
            var uiGo = _uiFactory.Create(UIType.UI_ENEMY_HEALTHBAR, unit.transform);
            if (uiGo != null) {
                var bar = uiGo.GetComponent<EnemyHealthBar>();
                var pos = unit as IPositionProvider;
                var attr = unit as IUnitAttribute<UnitAttribute>;
                if (bar != null && pos != null && attr != null) {
                    bar.Bind(pos, attr);
                }
            }
        }
        var bar = unit.GetComponentInChildren<EnemyHealthBar>();
        if (bar != null) {
            _uiFactory.Release(bar.gameObject);
        }
            var bar = unit.GetComponentInChildren<EnemyHealthBar>();
            if (bar != null) {
                _uiFactory.Release(bar.gameObject);
            }
    [TabGroup("Unit Factory")][Tooltip("UnitData SO ↔ Prefab 매핑")][SerializeField] private List<UnitPrefabEntry> _entries = new List<UnitPrefabEntry>();

    private Dictionary<UnitBaseSO, GameObject> _map;
    private IPoolManager _pool;

    // 활성화된(생성된) 유닛 목록
    private readonly List<UnitBase> _activeUnits = new List<UnitBase>();

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

        // 활성 유닛으로 등록
        _activeUnits.Add(unit);

        return unit;
    }

    public void Release(UnitBase unit)
    {
        if (unit == null) return;
        
        // 활성 목록에서 제거
        if (_activeUnits.Contains(unit))
        {
            _activeUnits.Remove(unit);
        }

        unit.gameObject.SetActive(false);
        _pool.Return(unit.gameObject);
    }

    public void ReleaseAll()
    {
        for (int i = _activeUnits.Count - 1; i >= 0; i--)
        {
            var unit = _activeUnits[i];
            if (unit != null)
            {
                unit.gameObject.SetActive(false);
                _pool.Return(unit.gameObject);
            }
        }
        _activeUnits.Clear();
    }
}
