using UnityEngine;

public interface IUIOverlay
{
    void OverlayIn(GameObject page, SlideDirection inDirection);

    void OverlayOut(GameObject page, SlideDirection outDirection);

    void DisableAfter(GameObject page, float delay);
}
