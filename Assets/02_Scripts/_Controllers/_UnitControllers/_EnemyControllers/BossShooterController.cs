using UnityEngine;
using Sirenix.OdinInspector;

//==============================================================================
// BossShooterController
//==============================================================================
// 보스 전용 슈터 컨트롤러. 간단한 원형, 부채꼴, 핑퐁 원형 탄막 패턴을 사용한다.
//==============================================================================
public class BossShooterController : MonoBehaviour, IShoot
{
    [TabGroup("Timer")][SerializeField] private float _shootInterval;
    [TabGroup("Shooting")][SerializeField] private GameObject _bulletSpawnPoint;
    [TabGroup("Projectile")][SerializeField] private ProjectileBaseSO _bulletData;
    [TabGroup("Pattern")][SerializeField] private int _bulletCount = 16;
    [TabGroup("Pattern")][SerializeField] private float _fanAngle = 60f;

    private float _shootTimer;
    private bool _pingToggle;

    public void Shoot()
    {
        CirclePattern();
        FanPattern();
        PingPongCirclePattern();
    }

    private void Start()
    {
        if (_bulletSpawnPoint == null) _bulletSpawnPoint = this.gameObject;
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        _shootTimer += Time.deltaTime;
        if (_shootTimer >= _shootInterval)
        {
            _shootTimer = 0f;
            Shoot();
        }
    }

    // 원형 탄막 패턴
    private void CirclePattern()
    {
        float step = 360f / _bulletCount;
        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = step * i;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            AllManager.instance.ProjectileFactory.Create(_bulletData, _bulletSpawnPoint.transform.position, rot);
        }
    }

    // 부채꼴 탄막 패턴
    private void FanPattern()
    {
        GameObject player = AllManager.instance.GamemodeManager.Player;
        if (player == null) return;
        Vector3 dir = (player.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        float start = baseAngle - _fanAngle * 0.5f;
        float step = _fanAngle / (_bulletCount - 1);
        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = start + step * i;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            AllManager.instance.ProjectileFactory.Create(_bulletData, _bulletSpawnPoint.transform.position, rot);
        }
    }

    // 핑퐁 원형 탄막 패턴
    private void PingPongCirclePattern()
    {
        float step = 360f / _bulletCount;
        float start = _pingToggle ? 0f : step * 0.5f;
        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = start + step * i;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            AllManager.instance.ProjectileFactory.Create(_bulletData, _bulletSpawnPoint.transform.position, rot);
        }
        _pingToggle = !_pingToggle;
    }
}
