using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

//------------------------------------------------------------------------------
// ProjectileBase
//------------------------------------------------------------------------------
// 모든 투사체가 상속받아 공통 로직(이동, 수명, 충돌 등록/해제, 공격력 등)을 갖습니다.
//------------------------------------------------------------------------------
public abstract class ProjectileBase: MonoBehaviour , IProjectileAttribute<ProjectileAttribute> , ICollidable , IAttacker , IDead
{
    [Header("Projectile Base")]

    [TabGroup("Projectile")] [Tooltip("투사체 속성 (ScriptableObject에서 할당된 값)")][OdinSerialize] protected ProjectileAttribute _projectileAttributes;
    public ProjectileAttribute Attributes { get { return _projectileAttributes; }set { _projectileAttributes = value; }}

    [TabGroup("Collision")] [Tooltip("충돌 반지름")][SerializeField]private float _collisionRadius;
    public float CollisionRadius{ get { return _collisionRadius; }}

    // ICollidable
    public Vector3 Position => transform.position;

    // IAttacker
    public float AttackPower => Attributes.ProjectileDamage;

    private float _age;

    // 1) Start 에서 SetStatus 호출
    protected virtual void Start()
    {
        SetStatus();
    }

    // 2) Update 에서 이동 & 수명 체크
    protected virtual void Update()
    {
        Move();
        AgeCheck();
    }

    /// <summary>
    /// 투사체가 꺼질 때(또는 재사용 직전에) 반드시 불러야 할 초기화 로직
    /// - 충돌 반지름 설정
    /// - CollisionManager 등록
    /// - 수명 타이머 초기화
    /// </summary>
    public virtual void SetStatus()
    {
        // Attributes 로부터 충돌 반지름 세팅
        _collisionRadius = Attributes.CollisionRadius;

        // 충돌 시스템에 등록
        AllManager.instance.CollisionManager.RegisterCollidable(this);

        // 수명 초기화
        _age = 0f;
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
    protected virtual void AgeCheck()
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
        // 충돌 시스템에서 해제
        AllManager.instance.CollisionManager.UnregisterCollidable(this);

        // 풀로 반환
        ProjectileFactory.Instance.Release(gameObject);
    }

    /// <summary>
    /// 충돌 이후 추가 후처리를 위해 서브클래스가 오버라이드할 수 있는 위치
    /// (예: 폭발, 관통 카운트 감소 등)
    /// </summary>
    public virtual void OnPostCollision(ICollidable other)
    {

    }
}
