using UnityEngine;

[DisallowMultipleComponent]
public class AttackStatsProvider : MonoBehaviour, IAttackStats, ISetStatus
{
    private IUnitAttribute<UnitAttribute> _unitAttr;
    private IWeaponHolder _weaponHolder;
    private WeaponBase _targetWeapon;

    public void SetStatus()
    {
        _unitAttr = this.GetComponent<IUnitAttribute<UnitAttribute>>();
        _weaponHolder = this.GetComponent<IWeaponHolder>();
        _targetWeapon = null;
    }

    public void SetTargetWeapon(WeaponBase weapon)
    {
        _targetWeapon = weapon;
    }

    public float AttackPower
    {
        get
        {
            float baseDmg = _unitAttr.Attributes.UnitDamage;
            float total = baseDmg;
            if (_targetWeapon != null)
            {
                total += (baseDmg * _targetWeapon.Attributes.DamageMultiplier) +
                         _targetWeapon.Attributes.FlatDamage;
            }
            return total;
        }
    }

    public float AttackSpeed
    {
        get
        {
            float speed = _unitAttr.Attributes.UnitAttackSpeed;
            if (_targetWeapon != null)
            {
                speed *= _targetWeapon.Attributes.AttackSpeedMultiplier;
            }
            return speed;
        }
    }

    public int ProjectileCount
    {
        get
        {
            int cnt = _unitAttr.Attributes.UnitProjectileNumber;
            if (_targetWeapon != null)
            {
                cnt += _targetWeapon.Attributes.ProjectileCountBonus;
            }
            return cnt;
        }
    }

    public float AttackRange
    {
        get
        {
            float rng = _unitAttr.Attributes.UnitAttackRange;
            if (_targetWeapon != null)
            {
                rng += _targetWeapon.Attributes.RangeBonus;
            }
            return rng;
        }
    }

    public float AttackDuration
    {
        get
        {
            float dur = _unitAttr.Attributes.UnitAttackDuration;
            if (_targetWeapon != null)
            {
                dur += _targetWeapon.Attributes.WeaponDisableTime;
            }
            return dur;
        }
    }

    public float AttackCooldown
    {
        get
        {
            float cd = _unitAttr.Attributes.UnitAttackCoolDown;
            if (_targetWeapon != null)
            {
                cd += _targetWeapon.Attributes.AttackCooldown;
            }
            return cd;
        }
    }

    public float KnockbackForce
    {
        get
        {
            float kb = _unitAttr.Attributes.UnitKnockbackForce;
            if (_targetWeapon != null)
            {
                kb += _targetWeapon.Attributes.KnockbackForce;
            }
            return kb;
        }
    }

    public int WeaponDisableCount
    {
        get
        {
            int dc = 0;
            if (_targetWeapon != null)
            {
                dc += _targetWeapon.Attributes.WeaponDisableCount;
            }
            return dc;
        }
    }
}
