using UnityEngine;

//==============================================================================
// IUISlider
//==============================================================================
// UI가 슬라이드 하는 효과를 위한 인터페이스
// 
//==============================================================================

public interface IUISlider
{
    // 주어진 방향으로 슬라이드 시작
    void Slide(SlideDirection direction);

    // 현재 슬라이드 중인지 여부
    bool IsSliding { get; }
}

public enum SlideDirection
{
    SLIDE_UP = 0,
    SLIDE_DOWN,
    SLIDE_LEFT,
    SLIDE_RIGHT
}