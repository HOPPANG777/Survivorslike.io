using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour, ICollidable, ISetStatus, IDead, ISpeakable
{
    [TabGroup("Item Base")][Tooltip("Item SO 데이터")][SerializeField] private ItemBaseSO _itemData;
    public ItemBaseSO ItemData { get { return _itemData; } set { _itemData = value; } }

    [TabGroup("Item Base")][Tooltip("SpriteRenderer")][SerializeField] private SpriteRenderer _renderer;

    public Vector3 Position { get { return this.transform.position; } }
    public float CollisionRadius { get { return _itemData.CollisionRadius; } set { /*noop*/ } }

    [Header("Sound")]
    // 사용할 사운드 이름
    [TabGroup("Sound")][Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [TabGroup("Sound")][Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [TabGroup("Sound")][Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    // 오디오를 출력할 주체
    [TabGroup("Sound")][Tooltip("오디오를 출력할 주체")][SerializeField] protected AudioSource _soundSource;
    public AudioSource SoundSource { get { return _soundSource; } }

    [TabGroup("Item Base")][Tooltip("Player")][SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } }

    private Vector3 _initialScale;
    //private float _pickupDistance = 0.1f;
    private bool _isAttracting;

    [TabGroup("Item Base")][Tooltip("BaseAttractRange")][SerializeField] private float _baseAttractRange;
    protected virtual float CurrentAttractRange { get { return _baseAttractRange; } }    

    virtual protected void Update()
    {
        if (AllManager.instance.GamemodeManager.Player == null) return;
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return; 

        PlayerGetItem();
    }

    public virtual void SoundPlay()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        if (_soundSource != null && _soundClip != null)
        {
            _soundSource.clip = _soundClip;
            AudioSource.PlayClipAtPoint(SoundClip, Position);
        }
    }

    public virtual void SoundStop()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;   
        if (_soundSource != null && _soundSource.isPlaying)
        {
            _soundSource.Stop();
        }
    }

    public virtual void SetStatus()
    {
        // SO로부터 필드 초기화
        _itemData.SetData(_itemData);
        _renderer = GetComponent<SpriteRenderer>();

        _initialScale = Vector3.one;
        _isAttracting = false;

        var go = AllManager.instance.GamemodeManager.Player;
        if (go != null) _player = go.gameObject;

        _renderer.sprite = _itemData.Icon;
        this.transform.localScale = _initialScale;

        _baseAttractRange = _itemData.AttractRange;

        _soundName = _itemData.SoundName;
        _soundType = _itemData.SoundType;
        _soundClip = _itemData.SoundClip;
        _soundSource = this.gameObject.GetComponent<AudioSource>();
        _soundSource.clip = _soundClip;

        gameObject.SetActive(true);
        AllManager.instance.CollisionManager.RegisterCollidable(this);
        SoundManager.instance.RegisterSound(this.SoundName, this, this.SoundType);
    }

    virtual protected void Collect()
    {
        if (_player == null) return;
        SoundManager.instance.PlaySound(this.SoundName);
        _itemData.Apply(_player.gameObject);
        CheckDead();
    }

    virtual public void CheckDead()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Dead();
    }

    virtual public void Dead()
    {
        AllManager.instance.CollisionManager.UnregisterCollidable(this);
        //SoundManager.instance.UnregisterSound(this.SoundName, this);
        this.gameObject.SetActive(false);
        AllManager.instance.ItemFactory.Release(this);
    }

    virtual protected void PlayerGetItem()
    {
        if (_player == null) return;

        float dist = Vector3.Distance(this.transform.position, _player.transform.position);
        if (dist <= CurrentAttractRange) _isAttracting = true;
        if (!_isAttracting) return;

        Vector3 dir = (_player.transform.position - this.transform.position).normalized;
        //this.transform.position += dir * 0.05f;

        //if (this.transform.localScale.x > 0.01f) this.transform.localScale -= Vector3.one * Time.deltaTime * 5.0f;

        this.transform.position += dir * _itemData.AttractSpeed * Time.deltaTime;
        dist = Vector3.Distance(this.transform.position, _player.transform.position);

        if (dist < 0.1f) Collect();
    }
}

//float t = 1f - Mathf.Clamp01((dist - _pickupDistance) / (_itemData.AttractRange - _pickupDistance));
//float speed = Mathf.Lerp(0f, _itemData.AttractSpeed, t);
//this.transform.position += (_player.transform.position - this.transform.position).normalized * speed * Time.deltaTime;