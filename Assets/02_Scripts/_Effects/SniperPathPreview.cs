using UnityEngine;

/// <summary>
/// 스나이퍼 투사체의 예상 경로를 선으로 보여주는 컴포넌트
/// </summary>
public class SniperPathPreview : MonoBehaviour, IProjectilePathPreviewer
{
    [SerializeField, Tooltip("선의 두께")]
    private float _lineWidth = 0.05f;

    private LineRenderer _line;
    private Transform _owner;
    private float _length;
    private Vector3 _direction = Vector3.right;
    private bool _visible;

    // 컴포넌트 초기 설정
    private void Awake()
    {
        _line = this.gameObject.GetComponent<LineRenderer>();
        _line.positionCount = 2;
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.startWidth = _lineWidth;
        _line.endWidth = _lineWidth;
        _line.enabled = false;
    }

    // 미리보기 대상과 초기 길이 설정
    public void Initialize(Transform owner)
    {
        _owner = owner;
    }

    // 경로 갱신
    public void UpdatePath(Vector3 direction, float length)
    {
        _direction = direction.normalized;
        _length = length;
        if (!_visible || _owner == null) return;

        Vector3 start = _owner.position;
        Vector3 end = start + _direction * _length;
        _line.SetPosition(0, start);
        _line.SetPosition(1, end);
    }

    // 미리보기 표시
    public void Show()
    {
        _visible = true;
        _line.enabled = true;
    }

    // 미리보기 숨김
    public void Hide()
    {
        _visible = false;
        _line.enabled = false;
    }

    // 매 프레임 위치를 보정하여 선을 유지한다
    private void LateUpdate()
    {
        if (!_visible || _owner == null) return;

        Vector3 start = _owner.position;
        Vector3 end = start + _direction * _length;
        _line.SetPosition(0, start);
        _line.SetPosition(1, end);
    }
}