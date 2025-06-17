using UnityEngine;

public class BombWeaponProjectile : ProjectileBase
{
    [SerializeField] private float _rotationSpeed = 360f;

    private Vector3 _p1;
    private Vector3 _p2;
    private Vector3 _p3;
    private Vector3 _p4;
    private float _timer;
    private float _lifeTime;

    public void Initialize(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        _p1 = p1;
        _p2 = p2;
        _p3 = p3;
        _p4 = p4;
        _timer = 0f;
    }

    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        _lifeTime = Attributes.LifeTime;
        _timer = 0f;
    }

    protected override void Move()
    {
        _timer += Time.deltaTime;
        float t = _lifeTime > 0f ? Mathf.Clamp01(_timer / _lifeTime) : 1f;
        Vector3 pos = CalculateBezier(_p1, _p2, _p3, _p4, t);
        transform.position = pos;
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }

    private static Vector3 CalculateBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float value)
    {
        Vector3 a = Vector3.Lerp(p1, p2, value);
        Vector3 b = Vector3.Lerp(p2, p3, value);
        Vector3 c = Vector3.Lerp(p3, p4, value);

        Vector3 d = Vector3.Lerp(a, b, value);
        Vector3 e = Vector3.Lerp(b, c, value);

        return Vector3.Lerp(d, e, value);
    }

    public override void CheckDead()
    {
        base.CheckDead();
    }

    public override void Dead()
    {
        base.Dead();
    }

    public override void SoundPlay()
    {
        base.SoundPlay();
    }

    public override void SoundStop()
    {
        base.SoundStop();
    }
}
