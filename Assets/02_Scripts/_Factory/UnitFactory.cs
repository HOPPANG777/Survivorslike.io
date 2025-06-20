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
    private bool _initialized;

    private readonly List<UnitBase> _activeUnits = new List<UnitBase>();
    private readonly Dictionary<UnitBase, UnitHealthBarController> _unitBars = new Dictionary<UnitBase, UnitHealthBarController>();

    private void Start()
    {
        //Init();
    }

    public void Init()
    {
        if (_initialized) return;

        _uiFactory = UIFactory.instance;
        _pool = PoolManager.instance;
        _map = new Dictionary<UnitBaseSO, GameObject>(_entries.Count);

        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Data != null && e.Prefab != null) _map[e.Data] = e.Prefab;
        }

        _initialized = true;
    }

    public UnitBase Create(UnitBaseSO data, Vector3 position)
    {
        if (!_initialized)
        {
            Init();
        }
        if (!_map.TryGetValue(data, out var prefab)) return null;
        var go = _pool.Get(prefab);
        go.transform.position = position;

        var unit = go.GetComponent<UnitBase>();
        unit.UnitBaseData = data;
        unit.SetStatus();
  
        _activeUnits.Add(unit);
        if (data is UnitBaseSO)
        {
            var parent = UIPoolManager.instance != null ? UIPoolManager.instance.CanvasRoot : null;
            var uiGo = _uiFactory.Create(UIType.UI_UNIT_HEALTHBAR, parent);
            if (uiGo != null)
            {
                var bar = uiGo.GetComponent<UnitHealthBarController>();
                var pos = unit as IPositionProvider;
                var attr = unit as IUnitAttribute<UnitAttribute>;
                if (bar != null && pos != null && attr != null)
                {
                    if (unit is PlayerUnit) bar.Bind(pos, attr, new Color(0, 255, 0, 200));
                    else bar.Bind(pos, attr, new Color(255, 0, 0, 200));
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