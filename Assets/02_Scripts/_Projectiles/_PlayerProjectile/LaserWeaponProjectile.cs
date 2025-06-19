using UnityEngine;

public class LaserWeaponProjectile : ProjectileBase
{
    private Vector3 _direction;
    private Camera _camera;

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;
        _camera = Camera.main;
    }

    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_NONE;
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

        transform.position += _direction * Attributes.ProjectileSpeed * Time.deltaTime;

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
