using UnityEngine;

//==============================================================================
// GameModeController
//==============================================================================
// Game의 전반적인 진행을 담당하는 Controller
//
//==============================================================================
public class GameModeController : MonoBehaviour, IGameMode
{
    [Header("게임모드 관련")]
    [Tooltip("게임모드 타입")][SerializeField] private GameModeType _gameMode;
    public GameModeType GameMode { get { return _gameMode; } set { _gameMode = value; } }
    
    [Header("컨트롤러")]
    [Tooltip("타일맵 컨트롤러")][SerializeField] private TileMapController _tileMapController;
    public TileMapController TileMapController { get { return _tileMapController; } }

    [Tooltip("카메라 컨트롤러")][SerializeField] private CameraController _cameraController;
    public CameraController CameraController { get { return _cameraController; } }

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
                    TileMapController.TilemapMove();
                    CameraController.FollowCameraMove();
                }
                break;
            case GameModeType.GAMEMODE_GAMEPAUSE:
                {

                }
                break;
            case GameModeType.GAMEMODE_LEVELUP:
                {

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
                Debug.Log("에러! 에러! 에러!");
                break;
        }
    }

}
