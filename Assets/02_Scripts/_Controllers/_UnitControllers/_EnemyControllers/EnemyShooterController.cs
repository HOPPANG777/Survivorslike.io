using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyShooterController : MonoBehaviour, IShoot
{
    [TabGroup("Timer")][Tooltip("발사 간격 -> 유닛 데이터의 공격 속도를 사용")][SerializeField] private float _shootInterval;
    [TabGroup("Shooting")][Tooltip("탄환이 생성될 위치 (빈 GameObject로 자식에 추가)")][SerializeField] private GameObject _bulletSpawnPoint;
    [TabGroup("Projectile")][SerializeField] private ProjectileBaseSO _bulletData;
    [TabGroup("Interface")][SerializeField] private IUnitAttribute<UnitAttribute> _enemyAttribute;

    // 내부 타이머
    private float _shootTimer = 0f;

    private void Start()
    {
        if (CheckEnemyAttribute() == false) return;
        _bulletSpawnPoint = this.gameObject;
        _shootInterval = _enemyAttribute.Attributes.UnitAttackCoolDown;
    }

    private void Update()
    {
        _shootTimer += Time.deltaTime;
        if (_shootTimer >= _shootInterval)
        {
            _shootTimer -= _shootInterval;
            Shoot();
        }
    }

    private bool CheckEnemyAttribute()
    {
        if (this.TryGetComponent<IUnitAttribute<UnitAttribute>>(out var attribute))
        {
            _enemyAttribute = attribute;
            return true;
        }
        else
        {
            Debug.LogError("IEnemyAttribute 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    public void Shoot()
    {
        if (_bulletData == null || _bulletSpawnPoint == null)
        {
            Debug.LogError("EnemyShooter: Bullet Data 또는 SpawnPoint 할당 누락");
            return;
        }

        // 팩토리에서 꺼내 쓰기
        var go = ProjectileFactory.Instance.Create(_bulletData, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);

        if (go == null) return;
    }
}
