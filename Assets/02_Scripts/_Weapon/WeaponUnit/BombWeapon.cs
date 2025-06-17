using UnityEngine;

public class BombWeapon : WeaponBase
{
    [SerializeField] private ProjectileBaseSO _projectileData;

    private AttackStatsProvider _stats;
    private float _cooldownTimer;
    private bool _leftRightToggle;

    public override void SetStatus()
    {
        base.SetStatus();
        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();
        _stats.SetTargetWeapon(this);
        if (_projectileData == null) _projectileData = ProjectileFactory.instance.GetProjectileData(WeaponType);
        _cooldownTimer = 0f;
        _leftRightToggle = false;
    }

    public override void Update()
    {
        base.Update();
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        if (_stats != null) _stats.SetTargetWeapon(this);

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer += Time.deltaTime;
            if (_stats != null && _cooldownTimer >= _stats.AttackCooldown)
            {
                _cooldownTimer = 0f;
                Shoot();
            }
        }
        else
        {
            Shoot();
            _cooldownTimer += Time.deltaTime;
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        int count = Mathf.Max(1, _stats.ProjectileCount);
        for (int i = 0; i < count; i++)
        {
            var proj = ProjectileFactory.instance.Create(_projectileData, transform.position, Quaternion.identity, this) as BombWeaponProjectile;
            if (proj != null)
            {
                Vector3 p1 = transform.position;
                Vector3 p2 = p1 + new Vector3(0f, 5f, 0f);

                float x1 = UnityEngine.Random.Range(1f, 5f);
                if (_leftRightToggle) x1 = -x1;

                Vector3 p3 = p2 + new Vector3(x1, 0f, 0f);
                Vector3 p4 = p1 + new Vector3(x1, -10f, 0f);
                proj.Initialize(p1, p2, p3, p4);
                _leftRightToggle = !_leftRightToggle;
            }
            else if (proj == null)
            {
                Debug.LogError("Failed to create BombWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
    }
}
