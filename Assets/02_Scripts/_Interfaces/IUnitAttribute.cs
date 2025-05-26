using System;
using UnityEngine;
using Sirenix.OdinInspector;

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
    //[Header("Attribute")]
    [TabGroup("Attribute")][Tooltip("체력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _health;
    public float UnitHealth { get { return _health; } set { _health = value; } }

    [TabGroup("Attribute")][Tooltip("현재 체력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _currentHealth;
    public float UnitCurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    [TabGroup("Attribute")][Tooltip("최대 체력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _maxHealth;
    public float UnitMaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    [TabGroup("Attribute")][Tooltip("이동속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _moveSpeed;
    public float UnitMoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    [TabGroup("Attribute")][Tooltip("공격력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _damage;
    public float UnitDamage { get { return _damage; } set { _damage = value; } }

    [TabGroup("Attribute")][Tooltip("방어력(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _defense;
    public float UnitDefense { get { return _defense; } set { _defense = value; } }

    [TabGroup("Attribute")][Tooltip("치명타(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _criticalDamage;
    public float UnitCriticalDamage { get { return _criticalDamage; } set { _criticalDamage = value; } }

    [TabGroup("Attribute")][Tooltip("치명타 확률(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _criticalChance;
    public float UnitCritChance { get { return _criticalChance; } set { _criticalChance = value; } }

    [TabGroup("Attribute")][Tooltip("투사체")][SerializeField] private int _projectileNumber;
    public int UnitProjectileNumber { get { return _projectileNumber; } set { _projectileNumber = value; } }

    [TabGroup("Attribute")][Tooltip("공격 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _attackSpeed;
    public float UnitAttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    [TabGroup("Attribute")][Tooltip("공격 범위(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _attackRange;
    public float UnitAttackRange { get { return _attackRange; } set { _attackRange = value; } }

    [TabGroup("Attribute")][Tooltip("공격 지속시간(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _attackDuration;
    public float UnitAttackDuration { get { return _attackDuration; } set { _attackDuration = value; } }

    [TabGroup("Attribute")][Tooltip("공격 쿨타임(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _attackCoolDown;
    public float UnitAttackCoolDown { get { return _attackCoolDown; } set { _attackCoolDown = value; } }

    [TabGroup("Attribute")][Tooltip("넉백 파워(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _knockbackForce;
    public float UnitKnockbackForce { get { return _knockbackForce; } set { _knockbackForce = value; } }
}

[Serializable]
public struct PlayerableAttribute
{
    //public UnitAttribute Base;

    [TabGroup("PlayerableAttribute")][Tooltip("플레이어 경험치 획득 반경(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _playerGetExpRange;
    public float PlayerGetExpRange { get { return _playerGetExpRange; } set { _playerGetExpRange = value; } }

    [TabGroup("PlayerableAttribute")][Tooltip("운(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _luck;
    public float UnitLuck { get { return _luck; } set { _luck = (int)value; } }

    [TabGroup("PlayerableAttribute")][Tooltip("플레이어 현재 경험치")][SerializeField] private float _playerCurrentExp;
    public float PlayerCurrentExp { get { return _playerCurrentExp; } set { _playerCurrentExp = value; } }

    [TabGroup("PlayerableAttribute")][Tooltip("플레이어 레벨")][SerializeField] private float _playerLevel;
    public int PlayerLevel { get { return _playerLevel; } set { _playerLevel = value; } }
}

[Serializable]
public struct EnemyableAttribute
{
    //public UnitAttribute Base;

    [TabGroup("EnemyableAttribute")][Tooltip("적이 가지고 있는 경험치")][SerializeField] private float _enemyPushStrength;
    public float EnemyPushStrength { get { return _enemyPushStrength; } set { _enemyPushStrength = value; } }
}
