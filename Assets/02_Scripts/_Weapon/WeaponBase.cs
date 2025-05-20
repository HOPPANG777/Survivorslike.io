using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponBase : MonoBehaviour, ITeamMember, ISpeakable, ISetStatus, ICollidable, IDead, IWeaponAttribute<WeaponAttribute>//, IAttacker
{
    [TabGroup("Weapon Base")][Tooltip("Weapon Data")][SerializeField] protected WeaponBaseSO _weaponBaseData;
    public WeaponBaseSO WeaponBaseData { get { return _weaponBaseData; } set { _weaponBaseData = value; } }

    [TabGroup("Weapon Base")][Tooltip("무기 이름")][SerializeField] protected string _weaponName;
    public string WeaponName { get { return _weaponName; } set { _weaponName = value; } }

    [TabGroup("Weapon Base")][Tooltip("팀 타입")][SerializeField] protected TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    [TabGroup("Weapon Base")][Tooltip("무기 능력치")][SerializeField] protected WeaponAttribute _weaponAttribute;
    public WeaponAttribute Attributes { get { return _weaponAttribute; } set { _weaponAttribute = value; } }

    [Header("Weapon Sound")]
    // 사용할 사운드 이름
    [TabGroup("Weapon Sound")][Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [TabGroup("Weapon Sound")][Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [TabGroup("Weapon Sound")][Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    // 오디오를 출력할 주체
    [TabGroup("Weapon Sound")][Tooltip("오디오를 출력할 주체")][SerializeField] protected AudioSource _soundSource;
    public AudioSource SoundSource { get { return _soundSource; } }

    [Header("ICollidable")]
    [TabGroup("ICollidable")][Tooltip("객체의 포지션을 받는")][SerializeField] protected Vector3 _position => this.transform.position;
    public Vector3 Position { get { return _position; } }

    [TabGroup("ICollidable")][Tooltip("객체의 포지션을 받는(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] protected float _collisionRadius;
    public float CollisionRadius { get { return _collisionRadius; } }

    [Header("IAttacker")]
    //public float AttackPower { get { return Attributes.UnitDamage * Weapon.Attributes.DamageMultiplier + Weapon.Attributes.FlatDamage; } }

    [TabGroup("Weapon Dead")][Tooltip("죽었는지 확인")][SerializeField] private bool isDead = false;
    public bool IsDead { get { return isDead; } }

    public void Update()
    {
        
    }

    public void SoundPlay()
    {

    }

    public void SoundStop()
    {

    }

    public virtual void SetStatus()
    {
       //_weaponBaseData.SetData(WeaponBaseData);
       //_weaponAttribute = _weaponBaseData.WeaponAttribute;
       //_weaponName = _weaponBaseData.WeaponName;
       //_collisionRadius = _weaponBaseData.CollisionRadius;
       //_teamType = _weaponBaseData.TeamType;
       //_soundName = _weaponBaseData.SoundName;
       //_soundType = _weaponBaseData.SoundType;
       //_soundClip = _weaponBaseData.SoundClip;
       //_soundSource = this.gameObject.GetComponent<AudioSource>();
       //AllManager.instance.CollisionManager.RegisterCollidable(this);
       //isDead = false;
    }

    public virtual void CheckDead()
    {
        if (isDead) return;
        //if (_weaponBaseData.IsDead) isDead = true;
    }

    public virtual void Dead()
    { 
        isDead = true;
        AllManager.instance.CollisionManager.UnregisterCollidable(this);
        gameObject.SetActive(false);
    }

}
