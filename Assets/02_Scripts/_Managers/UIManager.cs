using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [TabGroup("Controller")][Tooltip("타이머 컨트롤러")][SerializeField] private IngameClockController _ingameClockController;
    public IngameClockController IngameClockController { get { return _ingameClockController; } }

    [TabGroup("Controller")][Tooltip("플레이어 체력 바 컨트롤러")][SerializeField] private PlayerCurrentHealthBarController _playerCurrentHealthBarController;
    public PlayerCurrentHealthBarController PlayerCurrentHealthBarController { get { return _playerCurrentHealthBarController; } }

    [TabGroup("Controller")][Tooltip("플레이어 경험치 바 컨트롤러")][SerializeField] private PlayerCurrentExpBarController _playerCurrentExpBarController;
    public PlayerCurrentExpBarController PlayerCurrentExpBarController { get { return _playerCurrentExpBarController; } }

    [TabGroup("UIPage")][Tooltip("Back Manager")][SerializeField] private UIBackController _backManager;
    [TabGroup("UIPage")][Tooltip("Page List")][SerializeField] private List<GameObject> _pageList;

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

    public void ResetUI()
    {
        PlayerCurrentHealthBarController.UpdatePlayerHealthBarImage(1);
        _playerCurrentExpBarController.UpdatePlayerCurrentExpBar(0);
    }
}
