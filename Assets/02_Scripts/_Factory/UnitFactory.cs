using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitFactory : SerializedMonoBehaviour, IUnitFactory
{
    [TabGroup("Unit Factory")][Tooltip("UnitData SO  Prefab ")][SerializeField] private List<UnitPrefabEntry> _entries = new List<UnitPrefabEntry>();

    private Dictionary<UnitBaseSO, GameObject> _map;
    private IPoolManager _pool;
    private IUIFactory _uiFactory;
    private bool _initialized;

    // 현재 활성화된 유닛 리스트
    private readonly List<UnitBase> _activeUnits = new List<UnitBase>();
    // 생성된 체력 바 UI를 관리하기 위한 딕셔너리
    private readonly Dictionary<UnitBase, IHealthBarUI> _unitBars = new Dictionary<UnitBase, IHealthBarUI>();

    private void Start()
    {
        //Init();
    }

    public void Init()
    {
        if (_initialized) return;

        _uiFactory = AllManager.instance.UIFactory;
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
        if (data is UnitBaseSO && !(unit is ContainerUnit))
        {
            // 플레이어 여부에 따라 다른 UI 타입을 사용한다
            var parent = AllManager.instance.UIPoolCanvas != null ? AllManager.instance.UIPoolCanvas.CanvasRoot : null;
            UIType barType = unit is PlayerUnit ? UIType.UI_PLAYER_HEALTHBAR : UIType.UI_UNIT_HEALTHBAR;
            var uiGo = _uiFactory.Create(barType, parent);
            if (uiGo != null)
            {
                var bar = uiGo.GetComponent<IHealthBarUI>();
                var pos = unit as IPositionProvider;
                var attr = unit as IUnitAttribute<UnitAttribute>;
                if (bar != null && pos != null && attr != null)
                {
                    Color color = unit is PlayerUnit ? new Color(0, 255, 0, 200) : new Color(255, 0, 0, 200);
                    bar.Bind(pos, attr, color);
                    _unitBars[unit] = bar;
                }
            }
        }

        if (unit is BossUnit && unit is IUnitAttribute<UnitAttribute> bossAttr)
        {
            var bossBar = AllManager.instance.UIManager.BossCurrentHealthBarController;
            if (bossBar != null)
            {
                bossBar.Bind(bossAttr);
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
            // 인터페이스로 받은 객체가 MonoBehaviour라면 게임오브젝트 반환을 진행
            if (bar is MonoBehaviour mb)
            {
                _uiFactory.Release(mb.gameObject);
            }
            _unitBars.Remove(unit);
        }
        AllManager.instance.CollisionManager.UnregisterCollidable(unit);
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
                    if (bar is MonoBehaviour mb)
                    {
                        _uiFactory.Release(mb.gameObject);
                    }
                    _unitBars.Remove(unit);
                }
                AllManager.instance.CollisionManager.UnregisterCollidable(unit);
                unit.gameObject.SetActive(false);
                _pool.Return(unit.gameObject);
            }
        }
        _activeUnits.Clear();
        _unitBars.Clear();
    }

    public void ReleaseAllEnemies()
    {
        for (int i = _activeUnits.Count - 1; i >= 0; i--)
        {
            var unit = _activeUnits[i];
            if (unit == null) continue;

            if (unit is PlayerUnit || unit is ContainerUnit)
            {
                continue;
            }

            Release(unit);
        }
    }
}