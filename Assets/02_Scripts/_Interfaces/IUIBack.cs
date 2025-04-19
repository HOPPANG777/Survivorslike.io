using UnityEngine;

public interface IUIBack
{
    /// <summary>
    /// 페이지 전환 (현재 페이지 슬라이드 아웃, 새 페이지 슬라이드 인)
    /// </summary>
    /// <param name="page">이동할 페이지</param>
    /// <param name="outDirection">현재 페이지가 슬라이드 아웃할 방향</param>
    /// <param name="inDirection">새 페이지가 슬라이드 인할 방향</param>
    void NavigateTo(GameObject page, SlideDirection outDirection, SlideDirection inDirection);

    /// <summary>
    /// 이전 페이지로 되돌아감 (슬라이드 방향 역전)
    /// </summary>
    void GoBack();

    /// <summary>
    /// 이전으로 갈 수 있는지 여부
    /// </summary>
    bool CanGoBack { get; }
}
