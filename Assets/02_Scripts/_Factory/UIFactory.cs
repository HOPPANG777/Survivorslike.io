using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[DisallowMultipleComponent]
public class UIFactory : SerializedSingleton<UIFactory>, IUIFactory
{
    [TabGroup("UI Factory")]
    [SerializeField]
    private List<UIPrefabEntry> _entries = new List<UIPrefabEntry>();

    private IPoolManager _pool;
    private Dictionary<UIType, GameObject> _map;
    private readonly List<GameObject> _activeUIs = new List<GameObject>();

    private void Start()
    {
        _pool = PoolManager.instance;
        _map = new Dictionary<UIType, GameObject>(_entries.Count);
        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Prefab != null)
            {
                _map[e.Type] = e.Prefab;
            }
        }
    }

    public GameObject Create(UIType type, Transform parent = null)
    {
        if (!_map.TryGetValue(type, out var prefab)) return null;
        var go = _pool.Get(prefab);
        if (parent != null) go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        _activeUIs.Add(go);
        return go;
    }

    public void Release(GameObject uiObject)
    {
        if (uiObject == null) return;
        if (_activeUIs.Contains(uiObject)) _activeUIs.Remove(uiObject);
        uiObject.SetActive(false);
        _pool.Return(uiObject);
    }

    public void ReleaseAll()
    {
        for (int i = _activeUIs.Count - 1; i >= 0; i--)
        {
            var ui = _activeUIs[i];
            if (ui != null)
            {
                ui.SetActive(false);
                _pool.Return(ui);
            }
        }
        _activeUIs.Clear();
    }
}