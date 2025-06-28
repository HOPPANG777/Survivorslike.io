using UnityEngine;

//==============================================================================
// DroneWeapon
//==============================================================================
// 플레이어 주위를 회전하며 투사체를 발사하는 드론 무기
// Vampire Survivors 의 Peachone 처럼 베지어 곡선을 따라 투사체가 날아간다.
//==============================================================================

public class DroneWeapon : WeaponBase
{
    // 발사에 사용될 투사체 데이터
    [SerializeField]private ProjectileBaseSO _projectileData;

    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;

    // 발사 간격을 위한 쿨타임 타이머
    private float _cooldownTimer;

    // 플레이어 주위를 도는 각도 (라디안)
    private float _angle;

    // 드론 회전 속도(초당 각도)와 반경
    [SerializeField]private float _rotateSpeed = 180f;
    [SerializeField]private float _radius = 0.7f;

    //--------------------------------------------------------------------------
    // 무기 기본 설정
    //--------------------------------------------------------------------------
    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null)
        {
            _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();
        }

        if (_projectileData == null)
        {
            _projectileData = AllManager.instance.ProjectileFactory.GetProjectileData(WeaponType);
        }

        _stats.SetTargetWeapon(this);
        _cooldownTimer = 0f;
        _angle = 0f;
    }

    //--------------------------------------------------------------------------
    // 매 프레임 호출되는 업데이트
    //--------------------------------------------------------------------------
    public override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        base.Update();

        if (_stats == null) return;
        _stats.SetTargetWeapon(this);

        RotateAroundPlayer();

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer >= _stats.GetAttackCooldown(this))
            {
                _cooldownTimer = 0f;
                Shoot();
            }
        }
        else
        {
            // 첫 발사는 즉시 시행
            Shoot();
            _cooldownTimer += Time.deltaTime;
        }
    }

    //--------------------------------------------------------------------------
    // 플레이어 주위를 회전하며 위치 갱신
    //--------------------------------------------------------------------------
    private void RotateAroundPlayer()
    {
        if (this.transform.parent == null) return;

        _angle += Mathf.Deg2Rad * _rotateSpeed * Time.deltaTime;
        if (_angle > Mathf.PI * 2f) _angle -= Mathf.PI * 2f;

        Vector3 offset = new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0f) * _radius;
        this.transform.localPosition = offset;
    }

    //--------------------------------------------------------------------------
    // 투사체 발사
    //--------------------------------------------------------------------------
    public override void Shoot()
    {
        base.Shoot();

        if (_projectileData == null || _stats == null) return;

        int count = _stats.GetProjectileCount(this);
        float range = _stats.GetAttackRange(this);

        for (int i = 0; i < count; i++)
        {
            var proj = AllManager.instance.ProjectileFactory.Create
                (
                    _projectileData,
                    this.transform.position,
                    Quaternion.identity,
                    this
                ) as DroneWeaponProjectile;

            if (proj != null)
            {
                Vector3 center = this.transform.parent != null ? this.transform.parent.position : Vector3.zero;
                Vector3 start = this.transform.position;
                Vector3 endDir = (start - center).normalized;
                Vector3 end = center + endDir * range;

                Vector3 p2 = RandomPointAround(start, range * 0.5f);
                Vector3 p3 = RandomPointAround(end, range * 0.5f);

                proj.Initialize(start, p2, p3, end, _stats);
            }
            else
            {
                Debug.LogError("Failed to create DroneWeaponProjectile. Check prefab setting.");
            }
        }
    }

    //--------------------------------------------------------------------------
    // 기준 위치 주변 임의 포인트 계산
    //--------------------------------------------------------------------------
    private Vector3 RandomPointAround(Vector3 center, float radius)
    {
        float ang = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Cos(ang) * radius + center.x;
        float y = Mathf.Sin(ang) * radius + center.y;
        return new Vector3(x, y, 0f);
    }
}