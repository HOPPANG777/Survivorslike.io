using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [TabGroup("Controller")][Tooltip("타이머 컨트롤러")][SerializeField] private IngameClockController _ingameClockController;
    public IngameClockController IngameClockController { get { return _ingameClockController; } }

    [TabGroup("Controller")][Tooltip("플레이어 체력 바 컨트롤러")][SerializeField] private PlayerCurrentHealthBarController _playerCurrentHealthBarController;
    public PlayerCurrentHealthBarController PlayerCurrentHealthBarController { get { return _playerCurrentHealthBarController; } }

    [TabGroup("Controller")][Tooltip("플레이어 경험치 바 컨트롤러")][SerializeField] private PlayerCurrentExpBarController _playerCurrentExpBarController;
    public PlayerCurrentExpBarController PlayerCurrentExpBarController { get { return _playerCurrentExpBarController; } }

    [TabGroup("Controller")][Tooltip("적 킬 카운터 컨트롤러")][SerializeField] private EnemyKillCountController _enemyKillCountController;
    public EnemyKillCountController EnemyKillCountController { get { return _enemyKillCountController; } }

    [TabGroup("Controller")][Tooltip("플레이어 레벨 카운터 컨트롤러")][SerializeField] private PlayerCurrentLevelCountController _playerCurrentLevelCountController;
    public PlayerCurrentLevelCountController PlayerCurrentLevelCountController { get { return _playerCurrentLevelCountController; } }

    [TabGroup("Controller")][Tooltip("레벨업 아이템 이미지 슬라이드 컨트롤러")][SerializeField] private SlideInImagesController _slideInImagesController;
    public SlideInImagesController SlideInImagesController { get { return _slideInImagesController; } }

    [TabGroup("Controller")][Tooltip("레벨업시 아이템 선택 컨트롤러")][SerializeField] private WeaponSelectionController _weaponSelectionController;
    public WeaponSelectionController WeaponSelectionController { get { return _weaponSelectionController; } }


    [TabGroup("UIPage")][Tooltip("Back Manager")][SerializeField] private UIBackController _backManager;
    [TabGroup("UIPage")][Tooltip("Page List")][SerializeField] private List<GameObject> _pageList;

    public void Init()
    {
        _backManager.Init();
    }

    public void TitlePage()
    {
        _backManager.NavigateTo(_pageList[0], SlideDirection.SLIDE_UP, SlideDirection.SLIDE_DOWN);
    }

    public void SettingPage()
    {
        _backManager.NavigateTo(_pageList[1], SlideDirection.SLIDE_UP, SlideDirection.SLIDE_LEFT);
    }

    public void PausePage()
    {
        AllManager.instance.GamemodeManager.GamePause();
        _backManager.NavigateTo(_pageList[2], SlideDirection.SLIDE_DOWN, SlideDirection.SLIDE_UP);
    }

    public void IngamePage()
    {
        AllManager.instance.GamemodeManager.GameIngame();
        _backManager.NavigateTo(_pageList[3], SlideDirection.SLIDE_UP, SlideDirection.SLIDE_RIGHT);
    }

    public void KeyPage()
    {
        _backManager.NavigateTo(_pageList[4], SlideDirection.SLIDE_RIGHT, SlideDirection.SLIDE_LEFT);
    }

    public void LevelUpPage()
    {
        AllManager.instance.GamemodeManager.GameLevelUp();
        // Ingame 패널은 그대로, 레벨업 페이지만 overlay
        _backManager.OverlayIn(_pageList[5], SlideDirection.SLIDE_RIGHT);

        _weaponSelectionController.OpenSelection();

        // 이미지 슬라이드도 리셋→재생
        _slideInImagesController.SetStatus();
        _slideInImagesController.Move();
    }

    public void OnWeaponChosen()  // 무기 선택 콜백에서 호출
    {
        // 레벨업 패널만 겉으로 슬라이드 아웃
        _backManager.OverlayOut(_pageList[5], SlideDirection.SLIDE_LEFT);
    }

    public void GameOverPage()
    {
        AllManager.instance.GamemodeManager.GameOver();
        _backManager.NavigateTo(_pageList[6], SlideDirection.SLIDE_DOWN, SlideDirection.SLIDE_UP);
    }

    public void GameClearPage()
    {
        AllManager.instance.GamemodeManager.GameClear();
        _backManager.NavigateTo(_pageList[7], SlideDirection.SLIDE_DOWN, SlideDirection.SLIDE_UP);
    }

    public void ResetUI()
    {
        PlayerCurrentHealthBarController.UpdatePlayerHealthBarImage(1);
        _playerCurrentExpBarController.UpdatePlayerCurrentExpBar(0);
        EnemyKillCountController.UpdateKillCounter(0);
        _backManager.InitializeToTitle();
    }
}
