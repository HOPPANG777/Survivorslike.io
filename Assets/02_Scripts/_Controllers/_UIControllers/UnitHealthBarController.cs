using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 일반 유닛의 체력 바 UI를 제어하는 컨트롤러
/// </summary>
public class UnitHealthBarController : MonoBehaviour, IHealthBarUI
{
    // 실제 체력 값을 표시할 이미지
    [SerializeField] private Image _fillImage;
    // 유닛 기준 오프셋 값
    [SerializeField] private Vector3 _offset;
    // UI 카메라
    [SerializeField] private Camera _uiCamera;
    // 투명도 조절을 위한 캔버스 그룹
    [SerializeField] private CanvasGroup _canvasGroup;

    // 위치 정보를 제공하는 인터페이스
    private IPositionProvider _positionProvider;
    // 체력 정보를 제공하는 인터페이스
    private IUnitAttribute<UnitAttribute> _attributeProvider;
    // 바가 따라다닐 대상 오브젝트
    private GameObject _boundObject;
    // 원래 알파값(투명도) 저장용
    private float _originAlpha = 1f;

    /// <summary>
    /// 체력 바가 따라갈 대상과 색상을 설정한다
    /// </summary>
    /// <param name="positionProvider">월드 위치 제공 인터페이스</param>
    /// <param name="attributeProvider">체력 정보 제공 인터페이스</param>
    /// <param name="healthBarColor">체력 바 색상</param>
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

    // 초기 세팅
    private void Awake()
    {
        if (_uiCamera == null)
        {
            _uiCamera = Camera.main;
        }
        EnsureCanvasGroup();
        if (_canvasGroup != null) _originAlpha = _canvasGroup.alpha;
    }

    // 캔버스 그룹이 없을 경우 추가한다
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

    // 위치와 체력 값을 갱신한다
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
