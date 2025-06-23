using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBarController : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private CanvasGroup _canvasGroup;

    private IPositionProvider _positionProvider;
    private IUnitAttribute<UnitAttribute> _attributeProvider;
    private GameObject _boundObject;
    private float _originAlpha = 1f;

    public void Bind(IPositionProvider positionProvider, IUnitAttribute<UnitAttribute> attributeProvider, Color healthBarColor)
    {
        _positionProvider = positionProvider;
        _attributeProvider = attributeProvider;
        _fillImage.color = healthBarColor;
        EnsureCanvasGroup();
        _originAlpha = _canvasGroup != null ? _canvasGroup.alpha : 1f;
        if (_canvasGroup != null) _canvasGroup.alpha = 0f;

        _boundObject = null;
        if (positionProvider is MonoBehaviour mb)
        {
            _boundObject = mb.gameObject;
        }
        else if (attributeProvider is MonoBehaviour attrMb)
        {
            _boundObject = attrMb.gameObject;
        }
    }

    private void Awake()
    {
        if (_uiCamera == null)
        {
            _uiCamera = Camera.main;
        }
        EnsureCanvasGroup();
        if (_canvasGroup != null) _originAlpha = _canvasGroup.alpha;
    }

    private void EnsureCanvasGroup()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            _originAlpha = _canvasGroup.alpha;
        }
    }

    private void LateUpdate()
    {
        if (_positionProvider == null || _attributeProvider == null) return;

        if (_canvasGroup != null && _boundObject != null)
        {
            _canvasGroup.alpha = _boundObject.activeInHierarchy ? _originAlpha : 0f;
        }

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
