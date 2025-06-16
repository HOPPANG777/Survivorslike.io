using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitFactory : SerializedSingleton<UnitFactory>, IUnitFactory
{
    [TabGroup("Unit Factory")][Tooltip("UnitData SO  Prefab ")][SerializeField] private List<UnitPrefabEntry> _entries = new List<UnitPrefabEntry>();

    private Dictionary<UnitBaseSO, GameObject> _map;
    private IPoolManager _pool;
    private IUIFactory _uiFactory;

    private readonly List<UnitBase> _activeUnits = new List<UnitBase>();
    // Map each spawned unit to its health bar so we can properly release it
    private readonly Dictionary<UnitBase, EnemyHealthBar> _unitBars = new Dictionary<UnitBase, EnemyHealthBar>();

    private void Start()
    {
        _uiFactory = UIFactory.instance;
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
  
        _activeUnits.Add(unit);
        if (data is EnemyUnitSO)
        {
            var parent = UIPoolManager.instance != null ? UIPoolManager.instance.CanvasRoot : null;
            var uiGo = _uiFactory.Create(UIType.UI_ENEMY_HEALTHBAR, parent);
            if (uiGo != null)
            {
                var bar = uiGo.GetComponent<EnemyHealthBar>();
                var pos = unit as IPositionProvider;
                var attr = unit as IUnitAttribute<UnitAttribute>;
                if (bar != null && pos != null && attr != null)
                {
                    bar.Bind(pos, attr);
                    _unitBars[unit] = bar;
                }
            }
        }

        return unit;
    }

    public void Release(UnitBase unit)
    {
        if (unit == null) return;

        if (_activeUnits.Contains(unit))
        {
            _activeUnits.Remove(unit);
        }

        if (_unitBars.TryGetValue(unit, out var bar))
        {
            _uiFactory.Release(bar.gameObject);
            _unitBars.Remove(unit);
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
                if (_unitBars.TryGetValue(unit, out var bar))
                {
                    _uiFactory.Release(bar.gameObject);
                    _unitBars.Remove(unit);
                }
                unit.gameObject.SetActive(false);
                _pool.Return(unit.gameObject);
            }
        }
        _activeUnits.Clear();
        _unitBars.Clear();
    }
}