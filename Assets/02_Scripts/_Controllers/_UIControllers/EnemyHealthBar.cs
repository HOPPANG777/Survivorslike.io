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
    }

    private void Awake()
    {
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
            transform.position = _uiCamera.WorldToScreenPoint(worldPos);
        }
        else
        {
            transform.position = worldPos;
        }

        UnitAttribute attr = _attributeProvider.Attributes;
        if (attr.UnitMaxHealth > 0f)
        {
            _fillImage.fillAmount = attr.UnitCurrentHealth / attr.UnitMaxHealth;
        }
    }
}
