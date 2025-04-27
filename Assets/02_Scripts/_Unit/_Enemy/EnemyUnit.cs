using Sirenix.OdinInspector;
using UnityEngine;

//==============================================================================
// EnemyUnit
//==============================================================================
// EnemyUnit 스크립트
//	
//==============================================================================

public class EnemyUnit : UnitBase, IEnemyAttribute, IPushable
{
    [Header("Enemy Unit")]
    private EnemyUnitSO _enemyData;
    public EnemyUnitSO EnemyData { get { return _enemyData; } set { _enemyData = value; } }

    [TabGroup("Enemy Unit")][Tooltip("Enemy Attribute")][SerializeField] private EnemyableAttribute _enemyAttribute;
    public new EnemyableAttribute Attributes { get { return _enemyAttribute; } set { _enemyAttribute = value; } }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetStatus()
    {
        base.SetStatus();
        _enemyData = _unitBaseData as EnemyUnitSO;
        _enemyAttribute = _enemyData.EnemyAttribute;
    }

    public override void Hitted(float damageValue)
    {
        base.Hitted(damageValue);
    }

    public void Push(Vector3 direction)
    {
        // 캐싱된 거리 제곱값을 사용
        float sqrDis = direction.sqrMagnitude;

        // 충돌 반경 설정
        float minDistance = 1.0f;
        float minDistanceSquared = minDistance * minDistance;

        if (sqrDis < minDistanceSquared)
        {
            // 밀쳐내기 계산
            Vector3 pushDirection = direction.normalized * (minDistance - Mathf.Sqrt(sqrDis)) * Attributes.EnemyPushStrength;
            this.transform.position += pushDirection * 0.5f;
        }
    }

    public override void CheckDead()
    {
        base.CheckDead();
    }
    public override void Dead()
    {
        base.Dead();
        AllManager.instance.UIManager.EnemyKillCountController.UpdateKillCounter();
        UnitFactory.Instance.Release(this.gameObject.GetComponent<EnemyUnit>());
    }
}
