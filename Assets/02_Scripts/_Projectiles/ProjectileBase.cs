using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

//------------------------------------------------------------------------------
// ProjectileBase
//------------------------------------------------------------------------------
// 모든 투사체가 상속받아 공통 로직(이동, 수명, 충돌 등록/해제, 공격력 등)을 갖습니다.
//------------------------------------------------------------------------------
public abstract class ProjectileBase: MonoBehaviour , IProjectileAttribute<ProjectileAttribute> , ICollidable , IAttacker , IDead, ISetStatus
{
    [Header("Projectile Base")]
    [TabGroup("Projectile Base")][Tooltip("Projectile Data")][SerializeField] protected ProjectileBaseSO _projectileBaseData;
    public ProjectileBaseSO ProjectileBaseData { get { return _projectileBaseData; } set { _projectileBaseData = value; } }

    [TabGroup("Projectile")] [Tooltip("투사체 속성 (ScriptableObject에서 할당된 값)")][OdinSerialize] protected ProjectileAttribute _projectileAttributes;
    public ProjectileAttribute Attributes { get { return _projectileAttributes; }set { _projectileAttributes = value; }}

    [TabGroup("Collision")] [Tooltip("충돌 반지름")][SerializeField] private float _collisionRadius;
    public float CollisionRadius{ get { return _collisionRadius; }}

    [TabGroup("Projectile Base")][Tooltip("발사체 이름")][SerializeField] protected string _projectileName;
    public string ProjectileName { get { return _projectileName; } set { _projectileName = value; } }

    [TabGroup("Projectile Base")][Tooltip("팀 타입")][SerializeField] protected TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    [Header("Projectile Sound")]
    // 사용할 사운드 이름
    [TabGroup("Projectile Sound")][Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [TabGroup("Projectile Sound")][Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [TabGroup("Projectile Sound")][Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    // 오디오를 출력할 주체
    [TabGroup("Projectile Sound")][Tooltip("오디오를 출력할 주체")][SerializeField] protected AudioSource _soundSource;
    public AudioSource SoundSource { get { return _soundSource; } }

    [Header("ICollidable")]
    [TabGroup("ICollidable")][Tooltip("객체의 포지션을 받는")][SerializeField] protected Vector3 _position => this.transform.position;
    public Vector3 Position { get { return _position; } }

    //[Header("IAttacker")]
    public float AttackPower { get { return Attributes.ProjectileDamage; } }

    private float _age;

    private bool isDead = false;

    // 1) Start 에서 SetStatus 호출
    protected virtual void Start()
    {
        SetStatus();
    }

    // 2) Update 에서 이동 & 수명 체크
    protected virtual void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Move();
        CheckDead();
    }

    /// <summary>
    /// 투사체가 꺼질 때(또는 재사용 직전에) 반드시 불러야 할 초기화 로직
    /// - 충돌 반지름 설정
    /// - CollisionManager 등록
    /// - 수명 타이머 초기화
    /// </summary>
    public virtual void SetStatus()
    {
        _projectileBaseData.SetData(ProjectileBaseData);
        _projectileAttributes = _projectileBaseData.ProjectileAttribute;
        _projectileName = _projectileBaseData.ProjectileName;
        _teamType = _projectileBaseData.TeamType;
        _soundName = _projectileBaseData.SoundName;
        _soundType = _projectileBaseData.SoundType;
        _soundClip = _projectileBaseData.SoundClip;
        _soundSource = this.gameObject.GetComponent<AudioSource>();
        _collisionRadius = _projectileBaseData.CollisionRadius;
        AllManager.instance.CollisionManager.RegisterCollidable(this);
        _age = 0f;
        isDead = false;
    }

    /// <summary>
    /// 투사체 이동 로직 (transform.up 방향으로 보내줌)
    /// </summary>
    protected virtual void Move()
    {
        transform.position += transform.up * Attributes.ProjectileSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 수명 만료 시 자동 제거 (Dead 호출)
    /// </summary>
    public virtual void CheckDead()
    {
        _age += Time.deltaTime;
        if (_age >= Attributes.Lifetime)
        {
            Dead();
        }
    }

    /// <summary>
    /// IDead 구현: CollisionManager 에서 해제하고
    /// 풀 매니저로 반환
    /// </summary>
    public virtual void Dead()
    {
        if (isDead == true) return;

        // 충돌 시스템에서 해제
        AllManager.instance.CollisionManager.UnregisterCollidable(this);

        // 풀로 반환
        ProjectileFactory.instance.Release(this);

        isDead = true;
    }

    /// <summary>
    /// 충돌 이후 추가 후처리를 위해 서브클래스가 오버라이드할 수 있는 위치
    /// (예: 폭발, 관통 카운트 감소 등)
    /// </summary>
    public virtual void OnPostCollision(ICollidable other)
    {

    }
}
