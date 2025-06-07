using UnityEngine;
using System.Collections;

public interface IUIOverlay
{
    void OverlayIn(GameObject page, SlideDirection inDirection);

    void OverlayOut(GameObject page, SlideDirection outDirection);

    IEnumerator DisableAfter(GameObject page, float delay);
}
