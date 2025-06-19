using UnityEngine;

public class LaserWeaponProjectile : ProjectileBase
{
    private Vector3 _direction;
    private Camera _camera;
    private TrailRenderer _trail;
    private AttackStatsProvider _stats;

    // Count how many times this projectile has bounced off walls
    private int _wallHitCount;
    // Maximum number of wall collisions allowed before deactivation
    private int _maxWallHitCount;

    private void Awake()
    {
        _trail = gameObject.AddComponent<TrailRenderer>();
        ConfigureTrailRenderer();
    }

    protected override void Update()
    {
        base.Update();
    }

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

    public void Initialize(Vector3 direction, AttackStatsProvider stats)
    {
        _direction = direction.normalized;
        _camera = Camera.main;
        _stats = stats != null ? stats : OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
    }

    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_NONE;

        // Initialize wall hit counters based on projectile attributes
        _wallHitCount = 0;
        _maxWallHitCount = Attributes.PierceCount;
        // Prevent enemy collisions from disabling this projectile
        RemainingPierceCount = int.MaxValue;

        if (_trail != null)
        {
            _trail.Clear();
        }
    }

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

    public override void CheckDead()
    {
        UpdateCameraBound();

        if (RemainingLifeTime <= 0f || _wallHitCount >= _maxWallHitCount)
        {
            Dead();
        }
    }
}
