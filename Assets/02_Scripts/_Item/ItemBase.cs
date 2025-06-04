// File: ItemBase.cs
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour, ICollidable, ISetStatus, IDead
{
    [TabGroup("Item Base")][Tooltip("Item SO 데이터")][SerializeField] private ItemBaseSO _itemData;
    public ItemBaseSO ItemData { get { return _itemData; } set { _itemData = value; } }

    [TabGroup("Item Base")][Tooltip("SpriteRenderer")][SerializeField] private SpriteRenderer _renderer;

    public Vector3 Position { get { return transform.position; } }
    public float CollisionRadius { get { return _itemData.CollisionRadius; } set { /*noop*/ } }

    [TabGroup("Item Base")][Tooltip("Player")][SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } }

    private Vector3 _initialScale;
    //private float _pickupDistance = 0.1f;

    [TabGroup("Item Base")][Tooltip("BaseAttractRange")][SerializeField] private float _baseAttractRange;
    protected virtual float CurrentAttractRange { get { return _baseAttractRange; } }    

    virtual protected void Update()
    {
        if (_player == null) return;

        PlayerGetItem();
    }

    public virtual void SetStatus()
    {
        // SO로부터 필드 초기화
        _itemData.SetData(_itemData);
        _renderer = GetComponent<SpriteRenderer>();

        _initialScale = Vector3.one;

        var go = AllManager.instance.GamemodeManager.Player;
        if (go != null) _player = go.gameObject;

        _renderer.sprite = _itemData.Icon;
        this.transform.localScale = _initialScale;

        _baseAttractRange = _itemData.AttractRange;

        gameObject.SetActive(true);
        AllManager.instance.CollisionManager.RegisterCollidable(this);
    }



    virtual protected void Collect()
    {
        if (_player == null) return;
        _itemData.Apply(_player.gameObject);
        CheckDead();
    }

    virtual public void CheckDead()
    {
        Dead();
    }

    virtual public void Dead()
    {
        AllManager.instance.CollisionManager.UnregisterCollidable(this);
        this.gameObject.SetActive(false);
        ItemFactory.instance.Release(this);
    }

    virtual protected void PlayerGetItem()
    {
        if (_player == null) return;

        float dist = Vector3.Distance(this.transform.position, _player.transform.position);
        if (dist > CurrentAttractRange) return;

        Vector3 dir = (_player.transform.position - this.transform.position).normalized;
        this.transform.position += dir * 0.05f;

        if (this.transform.localScale.x > 0.01f) this.transform.localScale -= Vector3.one * 0.02f;

        if (dist < 0.1f) Collect();
    }
}

//float t = 1f - Mathf.Clamp01((dist - _pickupDistance) / (_itemData.AttractRange - _pickupDistance));
//float speed = Mathf.Lerp(0f, _itemData.AttractSpeed, t);
//this.transform.position += (_player.transform.position - this.transform.position).normalized * speed * Time.deltaTime;