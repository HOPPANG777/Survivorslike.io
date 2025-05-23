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
            foreach (var w in _weaponHolder.EquippedWeapons) total += (baseDmg * w.Attributes.DamageMultiplier) + w.Attributes.FlatDamage;
            return total;
        }
    }

    public float AttackSpeed
    {
        get
        {
            float speed = _unitAttr.Attributes.UnitAttackSpeed;
            foreach (var w in _weaponHolder.EquippedWeapons) speed *= w.Attributes.AttackSpeedMultiplier;
            return speed;
        }
    }

    public int ProjectileCount
    {
        get
        {
            int cnt = _unitAttr.Attributes.UnitProjectileNumber;
            foreach (var w in _weaponHolder.EquippedWeapons) cnt += w.Attributes.ProjectileCountBonus;
            return cnt;
        }
    }

    public float AttackRange
    {
        get
        {
            float rng = _unitAttr.Attributes.UnitAttackRange;
            foreach (var w in _weaponHolder.EquippedWeapons) rng += w.Attributes.RangeBonus;
            return rng;
        }
    }

    public float AttackDuration
    {
        get
        {
            float dur = _unitAttr.Attributes.UnitAttackDuration;
            foreach (var w in _weaponHolder.EquippedWeapons) dur += w.Attributes.WeaponDisableTime;
            return dur;
        }
    }

    public float AttackCooldown
    {
        get
        {
            float cd = _unitAttr.Attributes.UnitAttackCoolDown;
            foreach (var w in _weaponHolder.EquippedWeapons) cd += w.Attributes.AttackCooldown;
            return cd;
        }
    }

    public float KnockbackForce
    {
        get
        {
            float kb = _unitAttr.Attributes.UnitKnockbackForce;
            foreach (var w in _weaponHolder.EquippedWeapons) kb += w.Attributes.KnockbackForce;
            return kb;
        }
    }

    public int WeaponDisableCount
    {
        get
        {
            int dc = 0;
            foreach (var w in _weaponHolder.EquippedWeapons) dc += w.Attributes.WeaponDisableCount;
            return dc;
        }
    }
}

