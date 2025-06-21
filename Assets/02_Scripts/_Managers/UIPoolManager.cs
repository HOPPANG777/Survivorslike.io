using Sirenix.OdinInspector;
using UnityEngine;

[DisallowMultipleComponent]
public class UIPoolCanvasController : SerializedMonoBehaviour    
{
    [Tooltip("World-space canvas root for pooled UI elements")]
    public Transform CanvasRoot;
}
