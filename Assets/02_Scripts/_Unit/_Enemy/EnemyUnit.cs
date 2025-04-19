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
    [Tooltip("Enemy Data")][SerializeField] private EnemyUnitSO _enemyData;
    public EnemyUnitSO EnemyData { get { return _enemyData; } set { _enemyData = value; } }

    public EnemyableAttribute Attributes { get { return EnemyData.EnemyAttribute.Attributes; } set { EnemyData.EnemyAttribute.Attributes = value; } }
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
