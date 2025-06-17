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

    [TabGroup("Player")][Tooltip("플레이어")][SerializeField] private GameObject _player;
    private GameObject _playerInstance;
    private IPoolManager _pool;
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
        _pool = PoolManager.instance;
        if (_player != null)
        {
            _player.SetActive(false);
        }

        _gameTime = 0;
        _killCount = 0;
        RandomEnemySpawner.Init();
        //Player.GetComponent<PlayerUnit>().EquippedWeapons.Clear();

        if (_playerInstance != null)
        {
            _playerInstance.GetComponent<PlayerUnit>().EquippedWeapons.Clear();
            _pool.Return(_playerInstance);
        }

        _playerInstance = _pool.Get(_player);
        _playerInstance.transform.position = Vector3.zero;

        UnitFactory.instance.ReleaseAll();
        ProjectileFactory.instance.ReleaseAll();
        WeaponFactory.instance.ReleaseAll();
        AllManager.instance.UIManager.ResetUI();
        //Player.GetComponent<PlayerUnit>().SetStatus();
        //Player.transform.position = Vector3.zero;
        //Player.gameObject.SetActive(false);

        var unit = _playerInstance.GetComponent<PlayerUnit>();
        unit.SetStatus();
        unit.EquippedWeapons.Clear();

        _playerInstance.SetActive(false);

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
        //Player.GetComponent<PlayerUnit>().EquippedWeapons.Clear();
        if (_playerInstance != null)
        {
            _playerInstance.GetComponent<PlayerUnit>().EquippedWeapons.Clear();
            _pool.Return(_playerInstance);
            _playerInstance = null;
        }
        UnitFactory.instance.ReleaseAll();
        ProjectileFactory.instance.ReleaseAll();
        WeaponFactory.instance.ReleaseAll();
        AllManager.instance.UIManager.ResetUI();
        //Player.GetComponent<PlayerUnit>().SetStatus();
        //Player.transform.position = Vector3.zero;
        //Player.gameObject.SetActive(false);
        _player.SetActive(false); // keep prefab hidden
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
            _pool.Return(_playerInstance);
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
