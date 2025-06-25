using UnityEngine;

/// <summary>
/// 스나이퍼 무기 발사 전 경로를 표시하는 프리뷰 이펙트
/// </summary>
public class SniperPathPreview : MonoBehaviour, IProjectilePathPreviewer
{
    [SerializeField, Tooltip("라인 두께")]
    private float _lineWidth = 0.05f;
    [SerializeField, Tooltip("시작 지점 오프셋")]
    private float _startOffset = 0.2f;

    private LineRenderer _line;
    private Transform _owner;
    private float _length;
    private Vector3 _direction = Vector3.right;
    private bool _visible;
    private float _progress;

    // 초기화 시 라인렌더러 설정
    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 2;
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.startWidth = _lineWidth;
        _line.endWidth = _lineWidth;
        _line.enabled = false;
    }

    // 미리보기 대상 설정
    public void Initialize(Transform owner)
    {
        _owner = owner;
    }

    // 방향과 길이 갱신
    public void UpdatePath(Vector3 direction, float length)
    {
        _direction = direction.normalized;
        _length = length;
    }

    // 프리뷰 표시
    public void Show()
    {
        _visible = true;
        _progress = 0f;
        _line.enabled = true;
    }

    // 프리뷰 숨김
    public void Hide()
    {
        _visible = false;
        _line.enabled = false;
    }

    // 발사 준비 진행도 설정 (0~1)
    public void SetChargeProgress(float ratio)
    {
        _progress = Mathf.Clamp01(ratio);
    }

    // 매 프레임 라인 갱신
    private void LateUpdate()
    {
        if (!_visible || _owner == null) return;

        Vector3 start = _owner.position + _direction * _startOffset;
        Vector3 end = start + _direction * _length * _progress;
        _line.SetPosition(0, start);
        _line.SetPosition(1, end);
    }
}
