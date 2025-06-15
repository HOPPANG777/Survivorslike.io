using UnityEngine;

[DisallowMultipleComponent]
public class AttackStatsProvider : MonoBehaviour, IAttackStats, ISetStatus
{
    private IUnitAttribute<UnitAttribute> _unitAttr;
    private IWeaponHolder _weaponHolder;

    public void SetStatus()
    {
        _unitAttr = this.GetComponent<IUnitAttribute<UnitAttribute>>();
        _weaponHolder = this.GetComponent<IWeaponHolder>();
    }

    public float AttackPower
    {
        get
        {
            float baseDmg = _unitAttr.Attributes.UnitDamage;
            float total = baseDmg;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                var w = weapons[i];
                total += (baseDmg * w.Attributes.DamageMultiplier) + w.Attributes.FlatDamage;
            }
            return total;
        }
    }

    public float AttackSpeed
    {
        get
        {
            float speed = _unitAttr.Attributes.UnitAttackSpeed;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                speed *= weapons[i].Attributes.AttackSpeedMultiplier;
            }
            return speed;
        }
    }

    public int ProjectileCount
    {
        get
        {
            int cnt = _unitAttr.Attributes.UnitProjectileNumber;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                cnt += weapons[i].Attributes.ProjectileCountBonus;
            }
            return cnt;
        }
    }

    public float AttackRange
    {
        get
        {
            float rng = _unitAttr.Attributes.UnitAttackRange;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                rng += weapons[i].Attributes.RangeBonus;
            }
            return rng;
        }
    }

    public float AttackDuration
    {
        get
        {
            float dur = _unitAttr.Attributes.UnitAttackDuration;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                dur += weapons[i].Attributes.WeaponDisableTime;
            }
            return dur;
        }
    }

    public float AttackCooldown
    {
        get
        {
            float cd = _unitAttr.Attributes.UnitAttackCoolDown;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                cd += weapons[i].Attributes.AttackCooldown;
            }
            return cd;
        }
    }

    public float KnockbackForce
    {
        get
        {
            float kb = _unitAttr.Attributes.UnitKnockbackForce;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                kb += weapons[i].Attributes.KnockbackForce;
            }
            return kb;
        }
    }

    public int WeaponDisableCount
    {
        get
        {
            int dc = 0;
            var weapons = _weaponHolder.EquippedWeapons;
            for (int i = 0; i < weapons.Count; ++i)
            {
                dc += weapons[i].Attributes.WeaponDisableCount;
            }
            return dc;
        }
    }
}
