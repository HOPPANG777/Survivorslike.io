using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class UIBackController : MonoBehaviour, IUIBack, IUIOverlay
{
    private struct DisableEntry
    {
        public GameObject Page;
        public float Remaining;
    }
    private readonly List<DisableEntry> _disableEntries = new List<DisableEntry>();


    [TabGroup("UIPages")][Tooltip("등록할 UI 페이지들 (첫 번째는 시작 페이지)")][SerializeField] private List<GameObject> _pages;

    // 네비게이션 기록 구조체
    private struct NavEntry
    {
        public GameObject Page;
        public UISliderController Slider;
        public SlideDirection OutDirection;
        public SlideDirection InDirection;
    }

    [TabGroup("UIPages")][Tooltip("히스토리 스택")][SerializeField] private readonly Stack<NavEntry> _history = new Stack<NavEntry>();
    [TabGroup("UIPages")][Tooltip("현재 페이지")][SerializeField] private GameObject _currentPage;
    [TabGroup("UIPages")][Tooltip("현재 슬라이더")][SerializeField] private UISliderController _currentSlider;
    [TabGroup("UIPages")][Tooltip("생성시 포지션 리스트")][SerializeField] private List<Vector2> _initialPositions;

    public void Init()
    {
        // 모든 페이지 비활성화
        for (int i = 0; i < _pages.Count; i++) _pages[i].SetActive(false);

        // 초기 위치 리스트 생성
        _initialPositions = new List<Vector2>(_pages.Count);
        {
            // 초기 위치 수집 (레이아웃이 완료된 후 위치를 정확히 잡기 위해 Start에서 수행)
            _initialPositions = new List<Vector2>(_pages.Count);
            for (int i = 0; i < _pages.Count; i++)
            {
                var rect = _pages[i].GetComponent<RectTransform>();
                // 이 시점에 페이지가 비활성화되어도 RectTransform 값을 읽을 수 있음
                _initialPositions.Add(rect != null ? rect.anchoredPosition : Vector2.zero);
            }

            InitializeToTitle();
        }
    }

    /// <summary>
    /// UI를 초기 타이틀 상태로 리셋합니다.
    /// 게임 재시작 시 호출하세요.
    /// </summary>
    public void InitializeToTitle()
    {
        // 히스토리 클리어
        _history.Clear();

        // 모든 페이지 리셋: 위치 및 활성화
        for (int i = 0; i < _pages.Count; i++)
        {
            var page = _pages[i];
            var rect = page.GetComponent<RectTransform>();
            if (rect != null) rect.anchoredPosition = _initialPositions[i];

            page.SetActive(i == 0);
        }

        // 현재 페이지 참조 초기화
        _currentPage = _pages.Count > 0 ? _pages[0] : null;
        _currentSlider = _currentPage != null ? _currentPage.GetComponent<UISliderController>() : null;
    }

    /// <summary>
        DisableAfter(page, slider.MoveDuration);
    public void DisableAfter(GameObject page, float delay)
        _disableEntries.Add(new DisableEntry { Page = page, Remaining = delay });
        // 1) 위치 초기화
        var idx = _pages.IndexOf(page);
        var rect = page.GetComponent<RectTransform>();
        rect.anchoredPosition = _initialPositions[idx];

        // 2) 그냥 켜기 + 슬라이드
        page.SetActive(true);
        var slider = page.GetComponent<UISliderController>();
        slider.Slide(inDirection);
    }

    /// <summary>
    /// overlay로 띄운 page를 슬라이드 아웃하고, 비활성화합니다.
    /// </summary>
    public void OverlayOut(GameObject page, SlideDirection outDirection)
    {
        var slider = page.GetComponent<UISliderController>();
        slider.Slide(outDirection);
        // 슬라이드가 끝나는 시점을 알 수 있다면 콜백을 걸어서 비활성화하거나,
        // MoveDuration 후에 비활성화해 주세요:
        StartCoroutine(DisableAfter(page, slider.MoveDuration));
    }

    public IEnumerator DisableAfter(GameObject page, float delay)
    {
        yield return new WaitForSeconds(delay);
        page.SetActive(false);
    }

    public void NavigateTo(GameObject page, SlideDirection outDirection, SlideDirection inDirection)
    {
        // 같은 페이지 클릭 시 뒤로 가기
        if (page == _currentPage)
        {
            GoBack();
            return;
        }

        bool isCurrentRoot = _pages.Count > 0 && _currentPage == _pages[0];
        bool isTargetRoot = _pages.Count > 0 && page == _pages[0];
        bool isIngame = AllManager.instance.GamemodeManager.GameMode == GameModeType.GAMEMODE_GAMESTART;

        // 기록에 남기기
        _history.Push(new NavEntry { Page = _currentPage, Slider = _currentSlider, OutDirection = outDirection, InDirection = inDirection });

        // 현재 페이지 처리
        if (isCurrentRoot && !isIngame) _currentPage.SetActive(false);
        else _currentSlider.Slide(outDirection);

        // 목표 페이지 위치 초기화 후 활성화 및 슬라이드 인
        var nextRect = page.GetComponent<RectTransform>();
        if (nextRect != null) nextRect.anchoredPosition = _initialPositions[_pages.IndexOf(page)];

        page.SetActive(true);
        var nextSlider = page.GetComponent<UISliderController>();
        if (!(isTargetRoot && !isIngame)) nextSlider.Slide(inDirection);

        _currentPage = page;
        _currentSlider = nextSlider;
    }

    private void Update()
    {
        for (int i = _disableEntries.Count - 1; i >= 0; i--)
        {
            var entry = _disableEntries[i];
            entry.Remaining -= Time.deltaTime;
            if (entry.Remaining <= 0f)
            {
                if (entry.Page != null) entry.Page.SetActive(false);
                _disableEntries.RemoveAt(i);
            }
            else
            {
                _disableEntries[i] = entry;
            }
        }
    }

    [Button("뒤로 가기", ButtonSizes.Large)]
    public void GoBack()
    {
        if (_history.Count == 0) return;

        var entry = _history.Pop();
        bool previousIsRoot = _pages.Count > 0 && entry.Page == _pages[0];
        bool isIngame = AllManager.instance.GamemodeManager.GameMode == GameModeType.GAMEMODE_GAMESTART;

        // 현재 페이지 슬라이드 아웃
        _currentSlider.Slide(Opposite(entry.InDirection));

        // 이전 페이지 활성화
        entry.Page.SetActive(true);
        if (!(previousIsRoot && !isIngame)) entry.Slider.Slide(Opposite(entry.OutDirection));

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

