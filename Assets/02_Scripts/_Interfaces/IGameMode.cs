using UnityEngine;

//==============================================================================
// IGameMode
//==============================================================================
// 게임 모드의 상태들을 위한 인터페이스 해당 상태들은 게임의 전반적인 
// 상황을 지정함
//==============================================================================
public interface IGameMode
{
    public GameModeType GameMode { get; set; }
}

public enum GameModeType
{ 
    GAMEMODE_NONE = 0,
    GAMEMODE_IDLE,
    GAMEMODE_GAMESTART,
    GAMEMODE_INGAME,
    GAMEMODE_GAMEPAUSE,
    GAMEMODE_LEVELUP,
    GAMEMODE_GAMEOVER,
    GAMEMODE_GAMECLEAR
}

