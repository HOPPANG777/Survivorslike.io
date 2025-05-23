using UnityEngine;

public interface IAttackStats
{
    float AttackPower { get; }
    float AttackSpeed { get; }
    int ProjectileCount { get; }
    float AttackRange { get; }
    float AttackDuration { get; }
    float AttackCooldown { get; }
    float KnockbackForce { get; }
    int WeaponDisableCount { get; }
}
