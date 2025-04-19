using System;
using UnityEngine;

//==============================================================================
// IUnitAttribute
//==============================================================================
// 유닛의 능력치 인터페이스
//
//==============================================================================
public interface IUnitAttribute<Attribute>
{
    Attribute Attributes { get; set; }    
}

[Serializable]
public struct UnitAttribute
{
    [Header("Attribute")]

    [Tooltip("체력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _health;
    public float UnitHealth { get { return _health / 100f; } set { _health = (int)value; } }

    [Tooltip("현재 체력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _currentHealth;
    public float UnitCurrentHealth { get { return _currentHealth / 100f; } set { _currentHealth = (int)value; } }

    [Tooltip("최대 체력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _maxHealth;
    public float UnitMaxHealth { get { return _maxHealth / 100f; } set { _maxHealth = (int)value; } }

    [Tooltip("이동속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _moveSpeed;
    public float UnitMoveSpeed { get { return _moveSpeed / 100f; } set { _moveSpeed = (int)value; } }

    [Tooltip("공격력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _damage;
    public float UnitDamage { get { return _damage / 100f; } set { _damage = (int)value; } }

    [Tooltip("방어력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _defense;
    public float UnitDefense { get { return _defense / 100f; } set { _defense = (int)value; } }

    [Tooltip("치명타(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _criticalDamage;
    public float UnitCriticalDamage { get { return _criticalDamage / 100f; } set { _criticalDamage = (int)value; } }

    [Tooltip("치명타 확률(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _criticalChance;
    public float UnitCritChance { get { return _criticalChance / 100f; } set { _criticalChance = (int)value; } }

    [Tooltip("투사체")][SerializeField] private int _projectileNumber;
    public int UnitProjectileNumber { get { return _projectileNumber; } set { _projectileNumber = value; } }

    [Tooltip("투사체 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _projectileSpeed;
    public float UnitProjectileSpeed { get { return _projectileSpeed / 100f; } set { _projectileSpeed = (int)value; } }

    [Tooltip("공격 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _attackSpeed;
    public float UnitAttackSpeed { get { return _attackSpeed / 100f; } set { _attackSpeed = (int)value; } }

    [Tooltip("공격 범위(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _attackRange;
    public float UnitAttackRange { get { return _attackRange / 100f; } set { _attackRange = (int)value; } }

    [Tooltip("공격 지속시간(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _attackDuration;
    public float UnitAttackDuration { get { return _attackDuration / 100f; } set { _attackDuration = (int)value; } }

    [Tooltip("공격 쿨타임(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _attackCoolDown;
    public float UnitAttackCoolDown { get { return _attackCoolDown / 100f; } set { _attackCoolDown = (int)value; } }
}

[Serializable]
public struct PlayerableAttribute
{
    //public UnitAttribute Base;

    [Tooltip("플레이어 경험치 획득 반경(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _playerGetExpRange;
    public float PlayerGetExpRange { get { return _playerGetExpRange / 100f; } set { _playerGetExpRange = (int)value; } }

    [Tooltip("운(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _luck;
    public float UnitLuck { get { return _luck / 100f; } set { _luck = (int)value; } }

    [Tooltip("플레이어 현재 경험치")][SerializeField] private int _playerCurrentExp;
    public int PlayerCurrentExp { get { return _playerCurrentExp; } set { _playerCurrentExp = value; } }

    [Tooltip("플레이어 레벨")][SerializeField] private int _playerLevel;
    public int PlayerLevel { get { return _playerLevel; } set { _playerLevel = value; } }
}

[Serializable]
public struct EnemyableAttribute
{
    //public UnitAttribute Base;

    [Tooltip("적이 가지고 있는 경험치")][SerializeField] private int _enemyExp;
    public int EnemyExp { get { return _enemyExp; } set { _enemyExp = value; } }
}
