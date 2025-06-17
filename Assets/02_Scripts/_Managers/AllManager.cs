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

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 60;

        if (GamemodeManager == null) Debug.Log("GameModeContoller가 없습니다.");
        if (CollisionManager == null) Debug.Log("CollisionController가 없습니다.");
        if (UIManager == null) Debug.Log("UIController가 없습니다.");

        _uiController.Init();
        _collisionController.Init();

        PoolManager.instance.Init();
        UIFactory.instance.Init();
        UnitFactory.instance.Init();
        WeaponFactory.instance.Init();
        ProjectileFactory.instance.Init();

        GamemodeManager.GameStart();
    }
}
