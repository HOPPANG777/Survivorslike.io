using UnityEngine;

//==============================================================================
// EnemyUnitSO
//==============================================================================
// EnemyUnit Scriptable Object 기본적인 세팅을 빨리 하기 위한것
//	
//==============================================================================

[CreateAssetMenu(fileName = "EnemyData", menuName = "UnitData/EnemyData", order = 1)]

public class EnemyUnitSO : UnitBaseSO
{
    [Header("EnemyData")]
    [SerializeField] private IEnemyAttribute _enemyAttribute;
    public IEnemyAttribute EnemyAttribute { get { return _enemyAttribute; } set { _enemyAttribute = value; } }

    override public void SetData(UnitBaseSO _unitData)
    {
        base.SetData(_unitData);

        EnemyUnitSO _enemyData = _unitData as EnemyUnitSO;

        this.EnemyAttribute = _enemyData.EnemyAttribute;
    }
}