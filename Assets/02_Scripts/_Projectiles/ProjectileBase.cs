using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

//------------------------------------------------------------------------------
// ProjectileBase
//------------------------------------------------------------------------------
// 모든 투사체가 상속받아 공통 로직(이동, 수명, 충돌 등록/해제, 공격력 등)을 갖습니다.
//------------------------------------------------------------------------------
public abstract class ProjectileBase: MonoBehaviour , IProjectileAttribute<ProjectileAttribute> , ICollidable , IAttacker , IDead, ISetStatus, ISpeakable, ICameraBoundDetectable, IOutOfCameraKillable
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

    // 새로 추가: 이 투사체를 발사한 무기 참조
    public WeaponBase OwnerWeapon { get; private set; }

    [TabGroup("Weapon Base")][Tooltip("비활성화 타입")][SerializeField] private WeaponDisableType _disableType = WeaponDisableType.WEAPONDISABLETYPE_NONE;
    public WeaponDisableType DisableType { get { return _disableType; } set { _disableType = value; } }

    [TabGroup("Weapon Base")][Tooltip("비활성화 시간")][SerializeField] private float _remainingLifeTime;
    public float RemainingLifeTime { get { return _remainingLifeTime; } set { _remainingLifeTime = value; } }

    [TabGroup("Weapon Base")][Tooltip("비활성화 카운트")][SerializeField] private int _remainingPierceCount;
    public int RemainingPierceCount { get { return _remainingPierceCount; } set { _remainingPierceCount = value; } }

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

    [Header("IAttacker")]
    public float AttackPower { get; set; }

    protected float _age;

    private bool isDead = false;

    private bool _isOut = false;

    private float _outTimer = 0f;

    private float _killDelay = 3f;

    public bool IsOutOfCamera => _isOut;


    // 1) Start 에서 SetStatus 호출
    protected virtual void Start()
    {
        //SetStatus();
    }

    // 2) Update 에서 이동 & 수명 체크
    protected virtual void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        _remainingLifeTime -= Time.deltaTime;
        Move();
        CheckDead();
    }

    public virtual void SoundPlay()
    {

    }

    public virtual void SoundStop()
    {

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
        _remainingLifeTime = _projectileAttributes.LifeTime;
        _remainingPierceCount = _projectileAttributes.PierceCount;
        AttackPower = _projectileAttributes.ProjectileDamage;
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


    // (1) SetStatus 시 ownerWeapon을 함께 주입
    public void SetStatus(WeaponBase ownerWeapon)
    {
        OwnerWeapon = ownerWeapon;

        // SO 데이터 복제
        _projectileBaseData.SetData(_projectileBaseData);
        _projectileAttributes = _projectileBaseData.ProjectileAttribute;
        _projectileName = _projectileBaseData.ProjectileName;

        var statsProv = OwnerWeapon.GetComponent<AttackStatsProvider>();
        _projectileAttributes.LifeTime += statsProv.AttackDuration;
        _projectileAttributes.PierceCount += statsProv.WeaponDisableCount;
        
        // 내부 카운터 초기화
        _remainingLifeTime = _projectileAttributes.LifeTime;
        _remainingPierceCount = _projectileAttributes.PierceCount;
        AttackPower = statsProv.AttackPower;

        _teamType = _projectileBaseData.TeamType;
        _disableType = OwnerWeapon.WeaponDisableType;

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
        
    }

    /// <summary>
    /// 수명 만료 시 자동 제거 (Dead 호출)
    /// </summary>
    public virtual void CheckDead()
    {
        UpdateCameraBound();

        switch (_disableType)
        {
            case WeaponDisableType.WEAPONDISABLETYPE_NONE:
                {
                    // 이게 디폴트
                    _age += Time.deltaTime;
                    if (_age < _remainingLifeTime ) return;
                    Dead();
                }
                break;
            case WeaponDisableType.WEAPONDISABLETYPE_COUNT:
                {
                    // 무기 관통이나 반사를 통해서 카운트가 감소되서 0이 되면 사라지는 무기
                    if (_remainingLifeTime > 0) return;
                    Dead();
                }
                break;
            case WeaponDisableType.WEAPONDISABLETYPE_TIME:
                {
                    // 무기 사용시간이 지나면 사라지는 무기
                    if (_remainingPierceCount > 0) return;
                    Dead();
                }
                break;
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

    public void UpdateCameraBound()
    {
        bool nowOut = CheckOutOfCamera();
        if (nowOut && !_isOut)
        {
            _isOut = true;
            _outTimer = 0f;
            OnExitCamera();
        }
        else if (!nowOut && _isOut)
        {
            _isOut = false;
            _outTimer = 0f;
            OnEnterCamera();
        }

        // 밖에 있는 동안
        if (_isOut)
        {
            _outTimer += Time.deltaTime;
            OnOutOfCamera(_outTimer);
        }
    }

    public void OnExitCamera()
    {
        // 처음 나갈 때 동작(효과음 등)
        Debug.Log("카메라 밖으로 나감!");
    }

    public void OnEnterCamera()
    {
        // 다시 들어오면
        Debug.Log("카메라 안으로 들어옴!");
    }

    public void OnOutOfCamera(float outTime)
    {
        // 밖에 나가있는 시간 체크 후 죽이기
        if (outTime >= _killDelay) Dead();
    }

    private bool CheckOutOfCamera()
    {
        if (Camera.main == null) return false;
        Vector3 pos = transform.position;
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        return pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y;
    }
}
