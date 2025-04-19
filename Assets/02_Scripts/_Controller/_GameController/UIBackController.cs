using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class UIBackController : MonoBehaviour, IUIBack
{
    [TabGroup("UIPages")][Tooltip("등록할 UI 페이지들 (첫 번째는 시작 페이지)")][SerializeField] private List<GameObject> _pages;

    // 네비게이션 기록
    private struct NavEntry
    {
        public GameObject Page;
        public UISliderController Slider;
        public SlideDirection OutDirection;
        public SlideDirection InDirection;
    }

    [TabGroup("UIPages")][Tooltip("히스토리")][SerializeField] private readonly Stack<NavEntry> _history = new Stack<NavEntry>();
    [TabGroup("UIPages")][Tooltip("현재 페이지")][SerializeField] private GameObject _currentPage;
    [TabGroup("UIPages")][Tooltip("현제 슬라이더")][SerializeField] private UISliderController _currentSlider;

    void Awake()
    {
        // 등록된 모든 페이지 비활성화
        for (int i = 0; i < _pages.Count; i++)
        {
            _pages[i].SetActive(false);
        }
    }

    void Start()
    {
        // 첫 페이지 활성화
        if (_pages.Count > 0)
        {
            _currentPage = _pages[0];
            _currentSlider = _currentPage.GetComponent<UISliderController>();
            _currentPage.SetActive(true);
        }
    }

    public void NavigateTo(GameObject page, SlideDirection outDirection, SlideDirection inDirection)
    {
        // 같은 페이지 눌렀을 때는 GoBack()
        if (page == _currentPage)
        {
            GoBack();
            return;
        }

        // TitlePage 여부 판단
        bool isCurrentRoot = (_pages.Count > 0 && _currentPage == _pages[0]);
        bool isTargetRoot = (_pages.Count > 0 && page == _pages[0]);
        bool isIngame = AllManager.instance.GameMode.GameMode == GameModeType.GAMEMODE_INGAME;

        // 기록에 남기기
        _history.Push(new NavEntry
        {
            Page = _currentPage,
            Slider = _currentSlider,
            OutDirection = outDirection,
            InDirection = inDirection
        });

        // 현재 페이지 슬라이드 아웃 또는 비활성화
        if (isCurrentRoot && isIngame)
        {
            // 게임 모드 인게임일 때만 TitlePage 비활성화
            _currentPage.SetActive(false);
        }
        else
        {
            _currentSlider.Slide(outDirection);
        }

        // 목표 페이지 활성화 및 슬라이드 인 (TitlePage일 땐 정상 슬라이드)
        page.SetActive(true);
        var nextSlider = page.GetComponent<UISliderController>();
        if (!(isTargetRoot && isIngame))
        {
            nextSlider.Slide(inDirection);
        }

        _currentPage = page;
        _currentSlider = nextSlider;
    }

    [Button("뒤로 가기", ButtonSizes.Large)]
    public void GoBack()
    {
        if (_history.Count == 0) return;

        var entry = _history.Pop();
        bool previousIsRoot = (_pages.Count > 0 && entry.Page == _pages[0]);
        bool isIngame = AllManager.instance.GameMode.GameMode == GameModeType.GAMEMODE_INGAME;

        // 현재 페이지 슬라이드 아웃
        _currentSlider.Slide(Opposite(entry.InDirection));

        // 이전 페이지 활성화
        entry.Page.SetActive(true);

        // 이전 페이지가 TitlePage이면서 인게임 모드일 때만 비슬라이드, 그 외 정상 슬라이드
        if (!(previousIsRoot && isIngame))
        {
            entry.Slider.Slide(Opposite(entry.OutDirection));
        }

        _currentPage = entry.Page;
        _currentSlider = entry.Slider;
    }

    public bool CanGoBack => _history.Count > 0;

    private SlideDirection Opposite(SlideDirection dir)
    {
        switch (dir)
        {
            case SlideDirection.SLIDE_UP: return SlideDirection.SLIDE_DOWN;
            case SlideDirection.SLIDE_DOWN: return SlideDirection.SLIDE_UP;
            case SlideDirection.SLIDE_LEFT: return SlideDirection.SLIDE_RIGHT;
            case SlideDirection.SLIDE_RIGHT: return SlideDirection.SLIDE_LEFT;
            default: return dir;
        }
    }
}

