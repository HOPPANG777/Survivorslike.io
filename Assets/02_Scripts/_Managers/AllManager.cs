using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem.XR;

//==============================================================================
// AllManager
//==============================================================================
// 게임을 통제하는 컨트롤러에 접근 할 수 있도록 싱글톤을 이용해서 
// 만든 매니저 해당 매니저는 다양한 게임을 관리하는 여러 컨트롤러에 접근할 수 있는
// 유일한 통로
//==============================================================================
public class AllManager : Singleton<AllManager>
{
    [TabGroup("Controller")][SerializeField] private GameModeController _gameModeController;
    public GameModeController GamemodeManager { get { return _gameModeController; } }

    [TabGroup("Controller")][SerializeField] private CollisionController _collisionController;
    public CollisionController CollisionManager { get { return _collisionController; } }

    [TabGroup("Controller")][SerializeField] private UIController _uiController;
    public UIController UIManager { get { return _uiController; } }

    [TabGroup("Factory")][SerializeField] private UIFactory _uiFactory;
    public UIFactory UIFactory { get { return _uiFactory; } }

    [TabGroup("Factory")][SerializeField] private UnitFactory _unitFactory;
    public UnitFactory UnitFactory { get { return _unitFactory; } }

    [TabGroup("Factory")][SerializeField] private WeaponFactory _weaponFactory;
    public WeaponFactory WeaponFactory { get { return _weaponFactory; } }

    [TabGroup("Factory")][SerializeField] private ProjectileFactory _projectileFactory;
    public ProjectileFactory ProjectileFactory { get { return _projectileFactory; } }

    [TabGroup("Factory")][SerializeField] private ParticleFactory _particleFactory;
    public ParticleFactory ParticleFactory { get { return _particleFactory; } }

    [TabGroup("Factory")][SerializeField] private ItemFactory _itemFactory;
    public ItemFactory ItemFactory { get { return _itemFactory; } }

    [TabGroup("Canvas")][SerializeField] private UIPoolCanvasController _uIPoolCanvasController;
    public UIPoolCanvasController UIPoolCanvas { get { return _uIPoolCanvasController; } }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 60;

        if (GamemodeManager == null) Debug.Log("GameModeContoller가 없습니다.");
        if (CollisionManager == null) Debug.Log("CollisionController가 없습니다.");
        if (UIManager == null) Debug.Log("UIController가 없습니다.");
        if (UIFactory == null) Debug.Log("UIFactory가 없습니다.");
        if (UnitFactory == null) Debug.Log("UnitFactory가 없습니다.");
        if (WeaponFactory == null) Debug.Log("WeaponFactory가 없습니다.");
        if (ProjectileFactory == null) Debug.Log("ProjectileFactory가 없습니다.");
        if (ParticleFactory == null) Debug.Log("ParticleFactory가 없습니다.");
        if (ItemFactory == null) Debug.Log("ItemFactory가 없습니다.");
        if (UIPoolCanvas == null) Debug.Log("UIPoolCanvasController가 없습니다.");

        _uiController.Init();
        _collisionController.Init();
        PoolManager.instance.Init();
        UIFactory.Init();
        UnitFactory.Init();
        WeaponFactory.Init();
        ProjectileFactory.Init();
        ParticleFactory.Init();
        ItemFactory.Init();
        SoundManager.instance.CreatePlayers(SoundManager.instance.InitialPlayerCount); // 초기 사운드 플레이어 생성
        SoundManager.instance.InitVolumSliders();

        GamemodeManager.GameStart();
    }
}
