// File: ItemFactory.cs
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ItemFactory : SerializedMonoBehaviour, IItemFactory
{
    [TabGroup("Item Factory")]
    [Tooltip("Item SO ¡ê Prefab ¸ÅÇÎ")]
    [SerializeField]
    private List<ItemPrefabEntry> _entries = new List<ItemPrefabEntry>();

    private Dictionary<ItemBaseSO, GameObject> _map;
    private IPoolManager _pool;
    private List<ItemBase> _activeItems = new List<ItemBase>();

    private void Start()
    {
        //Init();
    }

    public void Init()
    {
        _pool = PoolManager.instance;
        _map = new Dictionary<ItemBaseSO, GameObject>(_entries.Count);
        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Data != null && e.Prefab != null)
            {
                _map[e.Data] = e.Prefab;
            }
        }
    }

    public ItemBase Create(ItemBaseSO data, Vector3 position)
    {
        if (!_map.TryGetValue(data, out var prefab)) return null;
        var go = _pool.Get(prefab);
        go.transform.position = position;
        go.transform.localScale = Vector3.one;

        var item = go.GetComponent<ItemBase>();
        item.ItemData = data;
        item.SetStatus();

        _activeItems.Add(item);
        
        return item;
    }

    public void Release(ItemBase item)
    {
        if (item == null) return;

        if (_activeItems.Contains(item))
        {
            _activeItems.Remove(item);
        }

        item.gameObject.SetActive(false);
        _pool.Return(item.gameObject);
    }

    public void ReleaseAll()
    {
        for (int i = _activeItems.Count - 1; i >= 0; i--)
        {
            var item = _activeItems[i];        
            if (item != null)
            {
                item.gameObject.SetActive(false);
                _pool.Return(item.gameObject);
            }
        }
        _activeItems.Clear();
    }

}
