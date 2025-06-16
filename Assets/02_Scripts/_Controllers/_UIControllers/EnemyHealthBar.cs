using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Vector3 _offset;
    // Camera used to convert world position to screen position.
    // Defaults to Camera.main if not explicitly set.
    [SerializeField] private Camera _uiCamera;

    private IPositionProvider _positionProvider;
    private IUnitAttribute<UnitAttribute> _attributeProvider;

    public void Bind(IPositionProvider positionProvider, IUnitAttribute<UnitAttribute> attributeProvider)
    {
        _positionProvider = positionProvider;
        _attributeProvider = attributeProvider;
        //_uiCamera = AllManager.instance.GamemodeManager.CameraController.MainCamera;
    }

    private void Awake()
    {
        // Fallback to Camera.main if no camera was assigned (e.g. when the
        // object is edited in Prefab Mode and cannot reference scene objects).
        if (_uiCamera == null)
        {
            _uiCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (_positionProvider == null || _attributeProvider == null) return;

        Vector3 worldPos = _positionProvider.Position + _offset;
        if (_uiCamera != null && GetComponentInParent<Canvas>()?.renderMode != RenderMode.WorldSpace)
        {
            this.transform.position = _uiCamera.WorldToScreenPoint(worldPos);
        }
        else
        {
            this.transform.position = worldPos;
        }

        UnitAttribute attr = _attributeProvider.Attributes;
        if (attr.UnitMaxHealth > 0f)
        {
            _fillImage.fillAmount = attr.UnitCurrentHealth / attr.UnitMaxHealth;
        }
    }
}
