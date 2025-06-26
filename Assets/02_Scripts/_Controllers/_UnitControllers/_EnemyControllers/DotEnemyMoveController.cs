using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 도트 스폰 시 직선으로 빠르게 이동하는 적 컨트롤러
/// 카메라 밖으로 나간 후 일정 시간 뒤에 제거된다
/// </summary>
public class DotEnemyMoveController : MonoBehaviour, IMoveable, ICameraBoundDetectable, IOutOfCameraKillable, ISetStatus, IDotMove
{
    [TabGroup("DotEnemyMove")][Tooltip("이동 속도")][SerializeField] private float _speed;
    [TabGroup("DotEnemyMove")][Tooltip("카메라 밖에서 제거되기까지 시간")][SerializeField] private float _killDelay;

    // 이동 방향
    private Vector3 _moveDirection;

    // 카메라 밖에 나갔는지 여부
    private bool _isOut;

    // 카메라 밖에 머문 시간
    private float _outTimer;

    // 적 사망 처리용 인터페이스
    private IDead _dead;

    // 적 능력치 인터페이스
    private IUnitAttribute<UnitAttribute> _attribute;

    /// <summary>
    /// 이동 방향 프로퍼티 구현
    /// </summary>
    public Vector3 MoveDirection { get => _moveDirection; set => _moveDirection = value; }

    /// <summary>
    /// 카메라 밖 여부 반환
    /// </summary>
    public bool IsOutOfCamera => _isOut;

    /// <summary>
    /// 초기 상태 설정
    /// </summary>
    public void SetStatus()
    {
        TryGetComponent<IDead>(out _dead);
        TryGetComponent<IUnitAttribute<UnitAttribute>>(out _attribute);
        if (_attribute != null)
        {
            _speed = _attribute.Attributes.UnitMoveSpeed;
        }
    }

    /// <summary>
    /// 이동 방향 지정
    /// </summary>
    public void SetDirection(Vector3 direction)
    {
        _moveDirection = direction.normalized;
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Move();
        UpdateCameraBound();
    }

    /// <summary>
    /// 실제 이동 처리
    /// </summary>
    public void Move()
    {
        transform.position += _moveDirection * _speed * Time.deltaTime;
    }

    /// <summary>
    /// 카메라 범위 체크
    /// </summary>
    public void UpdateCameraBound()
    {
        bool nowOut = CheckOutOfCamera();
        if (nowOut && !_isOut)
        {
            _isOut = true;
            _outTimer = 0f;
            OnExitCamera();
        }
        else if (!nowOut && _isOut)
        {
            _isOut = false;
            _outTimer = 0f;
            OnEnterCamera();
        }

        if (_isOut)
        {
            _outTimer += Time.deltaTime;
            OnOutOfCamera(_outTimer);
        }
    }

    public void OnExitCamera() { }
    public void OnEnterCamera() { }

    /// <summary>
    /// 카메라 밖에 머문 시간에 따라 제거
    /// </summary>
    public void OnOutOfCamera(float outTime)
    {
        if (outTime >= _killDelay)
        {
            _dead?.Dead();
        }
    }

    /// <summary>
    /// 화면 이탈 여부 계산
    /// </summary>
    private bool CheckOutOfCamera()
    {
        if (Camera.main == null) return false;
        Vector3 pos = transform.position;
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.nearClipPlane));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        return pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y;
    }
}
