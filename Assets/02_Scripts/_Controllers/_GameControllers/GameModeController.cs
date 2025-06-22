using Sirenix.OdinInspector;
using UnityEngine;

//==============================================================================
// GameModeController
//==============================================================================
// Game의 전반적인 진행을 담당하는 Controller
//
//==============================================================================
public class GameModeController : MonoBehaviour, IGameMode
{
    [TabGroup("GameMode")][Tooltip("게임모드 타입")][SerializeField] private GameModeType _gameMode;
    public GameModeType GameMode { get { return _gameMode; } set { _gameMode = value; } }
    
    [TabGroup("Controller")][Tooltip("타일맵 컨트롤러")][SerializeField] private TileMapController _tileMapController;
    public TileMapController TileMapController { get { return _tileMapController; } }

    [TabGroup("Controller")][Tooltip("카메라 컨트롤러")][SerializeField] private CameraController _cameraController;
    public CameraController CameraController { get { return _cameraController; } }

    [TabGroup("Controller")][Tooltip("스폰 컨트롤러")][SerializeField] private RandomEnemySpawner _randomEnemySpawner;
    public RandomEnemySpawner RandomEnemySpawner { get { return _randomEnemySpawner; } }

    [TabGroup("Game")][Tooltip("타이머")][SerializeField] private float _gameTime;
    public float GameTime { get { return _gameTime; } }

    [TabGroup("Game")][Tooltip("킬 카운트")][SerializeField] private int _killCount;
    public int KillCount { get { return _killCount; } set { _killCount = value; } }

    [TabGroup("Game")][Tooltip("무기 선택기")][SerializeField] private WeaponSelectionController _weaponSelectionController;
    public WeaponSelectionController WeaponSelectionController { get { return _weaponSelectionController; } }

    [TabGroup("Player")][Tooltip("Player Data")][SerializeField] private PlayerUnitSO _playerData;

    [TabGroup("Player")][Tooltip("Player")][SerializeField] private GameObject _playerInstance;
    public GameObject Player { get { return _playerInstance; } }

    private void Update()
    {
        switch (GameMode)
        {
            case GameModeType.GAMEMODE_NONE:
                {

                }
                break;
            case GameModeType.GAMEMODE_IDLE:
                {

                }
                break;
            case GameModeType.GAMEMODE_GAMESTART:
                {
                    if (TileMapController == null) Debug.Log("TileMapController가 참조되지 않았습니다!");
                    if (CameraController == null) Debug.Log("CameraController가 참조되지 않았습니다!");
                }
                break;
            case GameModeType.GAMEMODE_INGAME:
                {
                    _gameTime += Time.deltaTime;

                    TileMapController.Move();
                    CameraController.Move();
                    AllManager.instance.UIManager.IngameClockController.UpdateTimer(_gameTime);
                }
                break;
            case GameModeType.GAMEMODE_GAMEPAUSE:
                {

                }
                break;
            case GameModeType.GAMEMODE_LEVELUP:
                {
                    AllManager.instance.UIManager.PlayerCurrentExpBarController.UpdateExpBarBlinking();
                   
                }
                break;
            case GameModeType.GAMEMODE_GAMEOVER:
                {

                }
                break;
            case GameModeType.GAMEMODE_GAMECLEAR:
                {

                }
                break;
            default:
                Debug.Log("게임 모드 타입이 제대로 정의되지 않았습니다!");
                break;
        }
    }

    public void GameStart()
    {
        _gameTime = 0;
        _killCount = 0;
        RandomEnemySpawner.Init();

        if (_playerInstance != null)
        {
            var unitPrev = _playerInstance.GetComponent<PlayerUnit>();
            unitPrev.EquippedWeapons.Clear();
        }

        AllManager.instance.UnitFactory.ReleaseAll();
        AllManager.instance.ProjectileFactory.ReleaseAll();
        AllManager.instance.WeaponFactory.ReleaseAll();
        AllManager.instance.ItemFactory.ReleaseAll();
        AllManager.instance.UIFactory.ReleaseAll();
        AllManager.instance.UIManager.ResetUI();

        var playerUnit = AllManager.instance.UnitFactory.Create(_playerData, Vector3.zero) as PlayerUnit;
        _playerInstance = playerUnit != null ? playerUnit.gameObject : null;

        _tileMapController.Init();
        _cameraController.Init();
        _cameraController.SetOrthographicSize(10f);
        _weaponSelectionController.Init();

        if (playerUnit != null)
        {
            playerUnit.SetStatus();
            playerUnit.EquippedWeapons.Clear();
            _playerInstance.SetActive(false);
        }

        GameMode = GameModeType.GAMEMODE_GAMESTART;
    }

    public void GamePause()
    {
        GameMode = GameModeType.GAMEMODE_GAMEPAUSE;
    }

    public void GameIngame()
    {
        //Player.gameObject.SetActive(true);
        if (_playerInstance != null)
        {
            _playerInstance.SetActive(true);
        }

        GameMode = GameModeType.GAMEMODE_INGAME;
    }

    public void GameLevelUp()
    {
        GameMode = GameModeType.GAMEMODE_LEVELUP;
    }

    public void GameOver()
    {
        _gameTime = 0;
        _killCount = 0;
        RandomEnemySpawner.Init();

        if (_playerInstance != null)
        {
            var unit = _playerInstance.GetComponent<PlayerUnit>();
            unit.EquippedWeapons.Clear();
        }

        AllManager.instance.UnitFactory.ReleaseAll();
        AllManager.instance.ProjectileFactory.ReleaseAll();
        AllManager.instance.WeaponFactory.ReleaseAll();
        AllManager.instance.ItemFactory.ReleaseAll();
        AllManager.instance.UIFactory.ReleaseAll();
        AllManager.instance.UIManager.ResetUI();

        _playerInstance = null;

        GameMode = GameModeType.GAMEMODE_GAMEOVER;
    }

    public void GameClear()
    {
        GameMode = GameModeType.GAMEMODE_GAMECLEAR;
    }

    public void GameExit()
    {
        if (_playerInstance != null)
        {
            var unit = _playerInstance.GetComponent<PlayerUnit>();
            AllManager.instance.UnitFactory.Release(unit);
            _playerInstance = null;
        }
        GameMode = GameModeType.GAMEMODE_NONE;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
