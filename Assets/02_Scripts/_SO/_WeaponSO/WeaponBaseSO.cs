//==============================================================================
// WeaponBaseSO
//==============================================================================
// 무기 공통 데이터(ScriptableObject)  
// WeaponAttribute, 스프라이트, 이름, 사운드, 충돌 반경 등을 담고,
// ISetSOData 인터페이스로 런타임 복제 시 값을 복사할 수 있게 합니다.
//==============================================================================
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData/WeaponBaseSO", order = 0)]
public class WeaponBaseSO : ScriptableObject, ISetSOData<WeaponBaseSO>, IWeaponAttribute<WeaponAttribute>
{
    [TabGroup("Weapon Data")][Tooltip("무기 이름")][SerializeField] private string _weaponName; 
    public string WeaponName { get { return _weaponName; } set { _weaponName = value; } }
    
    [TabGroup("Weapon Data")][Tooltip("무기 아이콘")][SerializeField] private Sprite _weaponSprite; 
    public Sprite WeaponSprite { get { return _weaponSprite; } set { _weaponSprite = value; } }
    
    [TabGroup("Weapon Data")][Tooltip("무기 속성")][SerializeField] private WeaponAttribute _weaponAttribute; 
    public WeaponAttribute Attributes { get { return _weaponAttribute; } set { _weaponAttribute = value; } }

    [TabGroup("Weapon Data")][Tooltip("팀 타입")][SerializeField] protected TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    [TabGroup("Weapon Data")][Tooltip("팀 타입")][SerializeField] protected WeaponDisableType _weaponDisableType;
    public WeaponDisableType WeaponDisableType { get { return _weaponDisableType; } set { _weaponDisableType = value; } }

    [TabGroup("Sound")][Tooltip("사운드 이름")][SerializeField] private string _soundName; 
    public string SoundName { get { return _soundName; } }
    
    [TabGroup("Sound")][Tooltip("사운드 타입")][SerializeField] private SoundType _soundType; 
    public SoundType SoundType { get { return _soundType; } }
    
    [TabGroup("Sound")][Tooltip("오디오 클립")][SerializeField] private AudioClip _soundClip; 
    public AudioClip SoundClip { get { return _soundClip; } }
    
    [TabGroup("Collision")][Tooltip("충돌 반경")][SerializeField] private float _collisionRadius; 
    public float CollisionRadius { get { return _collisionRadius; } set { _collisionRadius = value; } }

    /// <summary>
    /// 다른 SO 인스턴스의 데이터를 이 인스턴스로 복사합니다.
    /// </summary>
    public virtual void SetData(WeaponBaseSO data)
    {
        this._weaponName = data._weaponName;

        this._weaponSprite = data._weaponSprite;

        this._teamType = data._teamType;

        this._weaponDisableType = data._weaponDisableType;

        this._weaponAttribute = data._weaponAttribute;

        this._soundName = data._soundName;

        this._soundType = data._soundType;

        this._soundClip = data._soundClip;

        this._collisionRadius = data._collisionRadius;
    }
}