using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

//==============================================================================
// UnitBase
//==============================================================================
// UnitBase 스크립트
//	
//==============================================================================

public abstract class UnitBase : MonoBehaviour, ITeamMember, ISpeakable, ISetStatus, ICollidable, IHitted, IAttacker, IUnitAttribute<UnitAttribute>, IDead
{
    [Header("Unit Base")]
    [TabGroup("Unit Base")][Tooltip("Unit Data")][SerializeField] protected UnitBaseSO _unitBaseData;
    public UnitBaseSO UnitBaseData { get { return _unitBaseData; } set { _unitBaseData = value; } }

    [TabGroup("Unit Base")][Tooltip("유닛 이름")][SerializeField] protected string _unitName;
    public string UnitName { get { return _unitName; } set { _unitName = value; } }

    [TabGroup("Unit Base")][Tooltip("팀 타입")][SerializeField] protected TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    [TabGroup("Unit Base")][Tooltip("유닛 능력치")][SerializeField] protected UnitAttribute _unitAttribute;
    public UnitAttribute Attributes { get { return _unitAttribute; } set { _unitAttribute = value; } }

    [Header("Unit Sound")]
    // 사용할 사운드 이름
    [TabGroup("Unit Sound")][Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [TabGroup("Unit Sound")][Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [TabGroup("Unit Sound")][Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    // 오디오를 출력할 주체
    [TabGroup("Unit Sound")][Tooltip("오디오를 출력할 주체")][SerializeField] protected AudioSource _soundSource;
    public AudioSource SoundSource { get { return _soundSource; } }

    [Header("ICollidable")]
    [TabGroup("ICollidable")][Tooltip("객체의 포지션을 받는")][SerializeField] protected Vector3 _position => this.transform.position;
    public Vector3 Position { get { return _position; } }

    [TabGroup("ICollidable")][Tooltip("객체의 포지션을 받는(그리고 100 분율로 보십시오 따라서 50일 0.5f가 됩니다.)")][SerializeField] protected float _collisionRadius;
    public float CollisionRadius { get { return _collisionRadius; } }

    [Header("IAttacker")]
    public virtual float AttackPower { get { return Attributes.UnitDamage; } }

    [TabGroup("Unit Dead")][Tooltip("죽었는지 확인")][SerializeField] private bool isDead = false;
    public bool IsDead { get { return isDead; } }

    [TabGroup("Unit Base")][Tooltip("피격 후 무적 지속 시간 (초)")][SerializeField] protected float _invincibleDuration = 0.1f;

    // 다음에 맞을 수 있는 시점 (Time.time 기준)
    private float _nextHittableTime = 0f;


    protected virtual void Start()
    {
        SetStatus();
    }

    protected virtual void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        CheckDead();
    }

    public void SoundPlay()
    {

    }

    public void SoundStop()
    {

    }

    public virtual void SetStatus()
    {
        _unitBaseData.SetData(UnitBaseData);
        _unitAttribute = _unitBaseData.UnitAttribute;
        _unitName = _unitBaseData.UnitName;
        _collisionRadius = _unitBaseData.CollisionRadius;
        _teamType = _unitBaseData.TeamType;
        _soundName = _unitBaseData.SoundName;
        _soundType = _unitBaseData.SoundType;
        _soundClip = _unitBaseData.SoundClip;
        _soundSource = this.gameObject.GetComponent<AudioSource>();
        AllManager.instance.CollisionManager.RegisterCollidable(this);
        isDead = false;
        // 무적 타이머 초기화: 스폰 직후 즉시 피격 가능
        _nextHittableTime = Time.time;
    }

    public virtual void Hitted(float damageValue)
    {
        // 무적 중이면 무시
        if (Time.time < _nextHittableTime) return;

        if (damageValue == 0) return;

        // 피격 이펙트 실행
        var effect = this.GetComponent<UnitHitEffect>();
        if (effect != null)
        {
            effect.PlayEffect(this.transform.position, Vector3.zero);
        }

        // 실제 데미지 계산
        float damageReduction = damageValue * (100f / (100f + this.Attributes.UnitDefense));
        this._unitAttribute.UnitCurrentHealth -= damageReduction;

        var parent = AllManager.instance.UIPoolCanvas != null ? AllManager.instance.UIPoolCanvas.CanvasRoot : null;
        var go = AllManager.instance.UIFactory.Create(UIType.UI_DAMAGE_TEXT, parent);
        if (go != null)
        {
            var dmg = go.GetComponent<DamageTextController>();
            if (dmg != null)
            {
                Color color = this is PlayerUnit ? Color.white : Color.red;
                dmg.Play(damageReduction, this.transform.position, color);
            }
        }

        // 즉시 사망 체크
        CheckDead();

        // 무적 시간 설정
        _nextHittableTime = Time.time + _invincibleDuration;
    }

    public virtual void CheckDead()
    {
        if (this._unitAttribute.UnitCurrentHealth <= 0)
        {
            this._unitAttribute.UnitCurrentHealth = 0;
            Dead();
        }
    }

    public virtual void Dead()
    {
        if (isDead == true) return;
        AllManager.instance.CollisionManager.UnregisterCollidable(this);
        isDead = true;
    }
}
