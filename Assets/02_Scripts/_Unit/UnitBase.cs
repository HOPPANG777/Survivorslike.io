using UnityEngine;

//==============================================================================
// UnitBase
//==============================================================================
// UnitBase 스크립트
//	
//==============================================================================

public class UnitBase : MonoBehaviour, ITeamMember, ISpeakable, ISetStatus, ICollidable, IHitted, IAttacker, IUnitAttribute<UnitAttribute>
{
    [Header("Unit Base")]
    [Tooltip("Unit Data")][SerializeField] protected UnitBaseSO _unitBaseData;
    public UnitBaseSO UnitBaseData { get { return _unitBaseData; } }

    [Tooltip("유닛 이름")][SerializeField] protected string _unitName;
    public string UnitName { get { return _unitName; } set { _unitName = value; } }

    [Tooltip("팀 타입")][SerializeField] protected TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    [Tooltip("유닛 능력치")][SerializeField] protected UnitAttribute _unitAttribute;
    public UnitAttribute Attributes { get { return _unitAttribute; } set { _unitAttribute = value; } }

    [Header("Unit Sound")]
    // 사용할 사운드 이름
    [Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    // 오디오를 출력할 주체
    [Tooltip("오디오를 출력할 주체")][SerializeField] protected AudioSource _soundSource;
    public AudioSource SoundSource { get { return _soundSource; } }

    [Header("ICollidable")]
    [Tooltip("객체의 포지션을 받는")][SerializeField] protected Vector3 _position => this.transform.position;
    public Vector3 Position { get { return _position; } }

    [Tooltip("객체의 포지션을 받는(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] protected int _collisionRadius;
    public float CollisionRadius { get { return _collisionRadius / 100.0f; } }

    [Header("IAttacker")]
    public float AttackPower { get { return Attributes.UnitDamage; } }

    virtual protected void Start()
    {
        SetStatus();
    }

    virtual protected void Update()
    {
        
    }

    public void SoundPlay()
    {

    }

    public void SoundStop()
    {
       
    }

    virtual public void SetStatus()
    {
        _unitBaseData.SetData(UnitBaseData);
        _unitAttribute = _unitBaseData.UnitAttribute;
        _unitName = _unitBaseData.UnitName;
        _teamType = _unitBaseData.TeamType;
        _soundName = _unitBaseData.SoundName;
        _soundType = _unitBaseData.SoundType;
        _soundClip = _unitBaseData.SoundClip;
    }

    virtual public void Hitted(float damageValue)
    {
        var damageReduction = damageValue * (100f / (100f + this.Attributes.UnitDefense));

        this._unitAttribute.UnitCurrentHealth -= (damageReduction);
    }
}
