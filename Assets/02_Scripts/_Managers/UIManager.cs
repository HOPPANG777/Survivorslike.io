using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

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
}
