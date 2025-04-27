using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ProjectileData", order = 0)]
public class ProjectileBaseSO : ScriptableObject, ISetSOData<ProjectileBaseSO>
{
    [TabGroup("ProjectileBaseSO")][SerializeField] protected ProjectileAttribute _projectileAttribute;
    public ProjectileAttribute ProjectileAttribute { get { return _projectileAttribute; } set { _projectileAttribute = value; } }

    [TabGroup("ProjectileBaseSO")][Tooltip("Projectile 스프라이트")][SerializeField] protected Sprite _projectileSprite;
    public Sprite ProjectileSprite { get { return _projectileSprite; } set { _projectileSprite = value; } }

    [TabGroup("ProjectileBaseSO")][Tooltip("Projectile 이름")][SerializeField] protected string _projectileName;
    public string ProjectileName { get { return _projectileName; } set { _projectileName = value; } }

    [TabGroup("ProjectileBaseSO")][Tooltip("팀 타입")][SerializeField] protected TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    // 사용할 사운드 이름
    [TabGroup("Sound")][Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [TabGroup("Sound")][Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [TabGroup("Sound")][Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    [TabGroup("Collision")][Tooltip("충돌 반경")][SerializeField] protected float _collisionRadius;
    public float CollisionRadius { get { return _collisionRadius; } }

    public virtual void SetData(ProjectileBaseSO projectileData)
    {
        this.ProjectileAttribute = projectileData.ProjectileAttribute;
        this.ProjectileSprite = projectileData.ProjectileSprite;
        this.ProjectileName = projectileData.ProjectileName;
        this.TeamType = projectileData.TeamType;
        this._soundName = projectileData._soundName;
        this._soundType = projectileData._soundType;
        this._soundClip = projectileData._soundClip;
        this._collisionRadius = projectileData._collisionRadius;
    }
}

