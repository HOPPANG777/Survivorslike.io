using Sirenix.OdinInspector;
using UnityEngine;

[DisallowMultipleComponent]
public class UIPoolManager : SerializedMonoBehaviour    
{
    [Tooltip("World-space canvas root for pooled UI elements")]
    public Transform CanvasRoot;
}
