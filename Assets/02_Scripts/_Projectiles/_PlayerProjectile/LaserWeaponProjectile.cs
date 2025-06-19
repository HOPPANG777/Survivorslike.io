using UnityEngine;

public class LaserWeaponProjectile : ProjectileBase
{
    private Vector3 _direction;
    private Camera _camera;
    private TrailRenderer _trail;

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
        _trail.widthCurve = new AnimationCurve(
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

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;
        _camera = Camera.main;
    }

    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_NONE;

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

        this.transform.position += _direction * Attributes.ProjectileSpeed * Time.deltaTime;

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
        }
    }

    public override void CheckDead()
    {
        UpdateCameraBound();

        if (RemainingLifeTime <= 0f || RemainingPierceCount <= 0)
        {
            Dead();
        }
    }
}
