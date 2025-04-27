using Sirenix.OdinInspector;
using System;
using UnityEngine;

public interface IProjectileAttribute<Attribute>
{
    Attribute Attributes { get; set; }
}

[Serializable]
public struct ProjectileAttribute
{
    [TabGroup("Attribute")][Tooltip("무기별 공격력")][SerializeField] private float _damage;
    public float ProjectileDamage { get { return _damage; } set { _damage = value; } }

    [TabGroup("Attribute")][Tooltip("투사체 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private float _projectileSpeed;
    public float ProjectileSpeed { get { return _projectileSpeed; } set { _projectileSpeed = value; } }

    [TabGroup("Attribute")][Tooltip("투사체 존속 시간(초)")][SerializeField] private float _lifetime;
    public float Lifetime { get { return _lifetime; } set { _lifetime = value; } }

    [TabGroup("Attribute")][Tooltip("충돌 반경")][SerializeField] private float _collisionRadius;
    public float CollisionRadius { get { return _collisionRadius; } set { _collisionRadius = value; } }

    [TabGroup("Attribute")][Tooltip("넉백 세기")][SerializeField] private float _knockbackForce;
    public float KnockbackForce { get { return _knockbackForce; } set { _knockbackForce = value; } }

    [TabGroup("Attribute")][Tooltip("관통 횟수(0이면 관통 없음)")][SerializeField] private int _pierceCount;
    public int PierceCount { get { return _pierceCount; } set { _pierceCount = value; } }

    [TabGroup("Attribute")][Tooltip("폭발 반경(% 단위)")][SerializeField] private float _explosionRadius;
    public float ExplosionRadius { get { return _explosionRadius; } set { _explosionRadius = value; } }

    [TabGroup("Attribute")][Tooltip("추적 여부")][SerializeField] private bool _isHoming;
    public bool IsHoming { get { return _isHoming; } set { _isHoming = value; } }

    [TabGroup("Attribute")][Tooltip("추적 강도")][SerializeField] private float _homingStrength;
    public float HomingStrength { get { return _homingStrength; } set { _homingStrength = value; } }
}

[Serializable]
public struct  PlayerProjectileAttribute
{
    [TabGroup("Attribute")][Tooltip("무기별 공격력")][SerializeField] private float _damage;
    public float PlayerProjectileDamage { get { return _damage; } set { _damage = value; } }
}