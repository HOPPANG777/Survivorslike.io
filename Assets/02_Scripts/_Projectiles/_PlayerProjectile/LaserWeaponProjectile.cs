using UnityEngine;

// 벽에 부딪혀 튕기는 레이저 투사체

public class LaserWeaponProjectile : ProjectileBase
{
    // 현재 이동 방향
    private Vector3 _direction;
    // 화면 경계 확인용 카메라
    private Camera _camera;
    // 궤적을 표현할 트레일 렌더러
    private TrailRenderer _trail;
    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;

    // 벽과 충돌한 횟수
    private int _wallHitCount;
    // 최대 벽 충돌 허용 횟수
    private int _maxWallHitCount;

    // 컴포넌트 초기 세팅
    private void Awake()
    {
        _trail = gameObject.AddComponent<TrailRenderer>();
        ConfigureTrailRenderer();
    }

    // 매 프레임 처리
    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        base.Update();
    }

    // 트레일 렌더러 설정
    private void ConfigureTrailRenderer()
    {
        if (_trail == null) return;

        _trail.time = 1.0f;
        _trail.startWidth = 0.05f;
        _trail.endWidth = 0.01f;

        _trail.material = new Material(Shader.Find("Sprites/Default"));
        _trail.startColor = Color.white;
        _trail.endColor = Color.white;

        _trail.minVertexDistance = 0.1f;
        _trail.widthCurve = new AnimationCurve
            (
            new Keyframe(0f, 0.2f),
            new Keyframe(0.5f, 0f)
            );

        _trail.colorGradient = new Gradient
        {
            colorKeys = new[]
            {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(Color.white, 0.5f)
            },
            alphaKeys = new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        };
    }

    // 투사체 기본 정보 설정
    public void Initialize(Vector3 direction, AttackStatsProvider stats)
    {
        _direction = direction.normalized;
        _camera = Camera.main;
        _stats = stats != null ? stats : OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
    }

    // 기본 상태 설정
    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_NONE;

        _wallHitCount = 0;
        _maxWallHitCount = Attributes.PierceCount;
        RemainingPierceCount = int.MaxValue;

        if (_trail != null)
        {
            _trail.Clear();
        }
    }

    // 화면 경계를 체크하며 이동
    protected override void Move()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                return;
            }
        }

        this.transform.position += _direction * _stats.GetAttackSpeed(OwnerWeapon) * Time.deltaTime;

        Vector3 viewPos = _camera.WorldToViewportPoint(transform.position);

        bool bounced = false;

        if (viewPos.x <= 0f)
        {
            viewPos.x = 0f;
            _direction = Vector3.Reflect(_direction, Vector3.right);
            bounced = true;
        }
        else if (viewPos.x >= 1f)
        {
            viewPos.x = 1f;
            _direction = Vector3.Reflect(_direction, Vector3.right);
            bounced = true;
        }

        if (viewPos.y <= 0f)
        {
            viewPos.y = 0f;
            _direction = Vector3.Reflect(_direction, Vector3.up);
            bounced = true;
        }
        else if (viewPos.y >= 1f)
        {
            viewPos.y = 1f;
            _direction = Vector3.Reflect(_direction, Vector3.up);
            bounced = true;
        }

        if (bounced)
        {
            transform.position = _camera.ViewportToWorldPoint(viewPos);
            _wallHitCount++;
        }
    }

    // 수명이나 벽 충돌 횟수 초과 시 사망 처리
    public override void CheckDead()
    {
        UpdateCameraBound();

        if (RemainingLifeTime <= 0f || _wallHitCount >= _maxWallHitCount)
        {
            Dead();
        }
    }
}
