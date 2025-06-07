using Sirenix.OdinInspector;
using Unity.VisualScripting;
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

    public void Push(Vector3 delta)
    {
        // 실제 거리
        float dist = delta.magnitude;
        float minDistance = 1.0f;                  // 원하는 최소 간격
        float overlap = minDistance - dist;        // 겹친 정도

        if (overlap > 0f && dist > 0f)
        {
            Vector3 dir = delta / dist;            // 단위 방향 벡터
                                                   // 겹친 거리만큼 세기 곱해 반반씩 밀어냄
            Vector3 push = dir * (overlap * Attributes.EnemyPushStrength * 0.5f);
            transform.position += push;
        }
    }

    public override void CheckDead()
    {
        base.CheckDead();
    }
    public override void Dead()
    {
        base.Dead();
        AllManager.instance.GamemodeManager.KillCount++;
        AllManager.instance.UIManager.EnemyKillCountController.UpdateKillCounter(AllManager.instance.GamemodeManager.KillCount);
        var enemyExpController = this.GetComponent<EnemyExpDropController>();
        if (enemyExpController == null) return;
        enemyExpController.EnemyDropExp();
        UnitFactory.instance.Release(this.gameObject.GetComponent<EnemyUnit>());
    }
}
