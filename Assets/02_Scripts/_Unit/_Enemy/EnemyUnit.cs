using UnityEngine;

//==============================================================================
// EnemyUnit
//==============================================================================
// EnemyUnit 스크립트
//	
//==============================================================================

public class EnemyUnit : UnitBase, IEnemyAttribute
{
    [Header("Enemy Unit")]
    private EnemyUnitSO _enemyData;
    public EnemyUnitSO EnemyData { get { return _enemyData; } set { _enemyData = value; } }

    [Tooltip("Enemy Attribute")][SerializeField] private EnemyableAttribute _enemyAttribute;
    public new EnemyableAttribute Attributes { get { return _enemyAttribute; } set { _enemyAttribute = value; } }

    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        base.Update();
    }

    override public void SetStatus()
    {
        base.SetStatus();
        _enemyData = _unitBaseData as EnemyUnitSO;
        _enemyAttribute = _enemyData.EnemyAttribute;
    }
}
