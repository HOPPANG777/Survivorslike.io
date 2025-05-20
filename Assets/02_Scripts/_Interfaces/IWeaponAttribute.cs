using UnityEngine;
using Sirenix.OdinInspector;

public interface IWeaponAttribute<Attribute>
{
    Attribute Attributes { get; set; }
}

[System.Serializable]
public struct WeaponAttribute
{
    [Header("Weapon Attribute")]
    [TabGroup("Attribute")][Tooltip("추가 고정 데미지")][SerializeField] private float _flatDamage; 
    public float FlatDamage { get { return _flatDamage; } set { _flatDamage = value; } }
    
    [TabGroup("Attribute")][Tooltip("공격력 배수 (유닛 공격력 × 이 값)")][SerializeField] private float _damageMultiplier; 
    public float DamageMultiplier { get { return _damageMultiplier; } set { _damageMultiplier = value; } }
    
    [TabGroup("Attribute")][Tooltip("치명타 추가 데미지")][SerializeField] private float _criticalDamage; 
    public float CriticalDamage { get { return _criticalDamage; } set { _criticalDamage = value; } }
    
    [TabGroup("Attribute")][Tooltip("치명타 확률")][SerializeField] private float _criticalChance; 
    public float CriticalChance { get { return _criticalChance; } set { _criticalChance = value; } }
    
    [TabGroup("Attribute")][Tooltip("공격 속도 배수 (유닛 공격속도 × 이 값)")][SerializeField] private float _attackSpeedMultiplier; 
    public float AttackSpeedMultiplier { get { return _attackSpeedMultiplier; } set { _attackSpeedMultiplier = value; } }
    
    [TabGroup("Attribute")][Tooltip("투사체 개수 보너스")][SerializeField] private int _projectileCountBonus; 
    public int ProjectileCountBonus { get { return _projectileCountBonus; } set { _projectileCountBonus = value; } }
    
    [TabGroup("Attribute")][Tooltip("투사체 속도 보너스")][SerializeField] private float _projectileSpeedBonus; 
    public float ProjectileSpeedBonus { get { return _projectileSpeedBonus; } set { _projectileSpeedBonus = value; } }
    
    [TabGroup("Attribute")][Tooltip("사거리 보너스")][SerializeField] private float _rangeBonus; 
    public float RangeBonus { get { return _rangeBonus; } set { _rangeBonus = value; } }
}
