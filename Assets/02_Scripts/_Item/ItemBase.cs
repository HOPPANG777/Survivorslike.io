// File: ItemBase.cs
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour, ICollidable
{
    [TabGroup("Item Base")][Tooltip("Item SO µ•¿Ã≈Õ")][SerializeField] private ItemBaseSO _itemData;
    public ItemBaseSO ItemData { get { return _itemData; } set { _itemData = value; } }

    [TabGroup("Item Base")][Tooltip("SpriteRenderer")][SerializeField] private SpriteRenderer _renderer;

    public Vector3 Position { get { return transform.position; } }
    public float CollisionRadius { get { return _itemData.CollisionRadius; } set { /*noop*/ } }

    private Transform _player;
    private Vector3 _initialScale;
    private float _pickupDistance = 0.1f;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale;
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) _player = go.transform;
    }

    private void OnEnable()
    {
        AllManager.instance.CollisionManager.RegisterCollidable(this);
    }

    private void OnDisable()
    {
        AllManager.instance.CollisionManager.UnregisterCollidable(this);
    }

    public virtual void Initialize(ItemBaseSO data, Vector3 position)
    {
        _itemData = data;
        transform.position = position;
        _renderer.sprite = data.Icon;
        transform.localScale = _initialScale;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_player == null) return;
        float dist = Vector3.Distance(transform.position, _player.position);
        if (dist > _itemData.AttractRange) return;

        float t = 1f - Mathf.Clamp01((dist - _pickupDistance) / (_itemData.AttractRange - _pickupDistance));
        float speed = Mathf.Lerp(0f, _itemData.AttractSpeed, t);
        transform.position += (_player.position - transform.position).normalized * speed * Time.deltaTime;

        if (dist < _pickupDistance) Collect();
    }

    private void Collect()
    {
        _itemData.Apply(_player.gameObject);
        OnUse();
    }

    protected virtual void OnUse()
    {
        gameObject.SetActive(false);
        ItemFactory.instance.Release(this);
    }
}
