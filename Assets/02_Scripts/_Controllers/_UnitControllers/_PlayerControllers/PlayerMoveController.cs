using Sirenix.OdinInspector;
using UnityEngine;

//==============================================================================
// PlayerMoveController
//==============================================================================
// Player의 이동을 담당하는 Controller
//
//==============================================================================
public class PlayerMoveController : MonoBehaviour, IMoveable
{
    [TabGroup("Target")][SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } }

    [TabGroup("Target")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    [TabGroup("Interface")][SerializeField] private IUnitAttribute<UnitAttribute> _playerAttribute;
    public IUnitAttribute<UnitAttribute> PlayerAttribute { get { return _playerAttribute; } }

    [TabGroup("Dash")][Tooltip("대쉬 이동 거리(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _dashDistance; // 5f;
    public float DashDistance { get { return _dashDistance / 100f; } set { _dashDistance = (int)value; } }

    [TabGroup("Dash")][Tooltip("대쉬 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _dashSpeed; // 20f;
    public float DashSpeed { get { return _dashSpeed / 100f; } set { _dashSpeed = (int)value; } }

    [TabGroup("Dash")][Tooltip("대쉬 쿨타임(s)(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _dashCooldown; // 2f;
    public float DashCooldown { get { return _dashCooldown / 100f; } set { _dashCooldown = (int)value; } }

    // 내부 상태
    private bool _isDashing;
    private Vector3 _dashDir;
    private Vector3 _dashTarget;
    private float _dashTimer;

    private bool CheckPlayerMove()
    {
        if (_player == null)
        {
            Debug.LogError("Player가 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckPlayerAttribute()
    {
        if (_player.TryGetComponent<IUnitAttribute<UnitAttribute>>(out var attribute))
        {
            _playerAttribute = attribute;
            return true;
        }
        else
        {
            Debug.LogError("IPlayerAttribute 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    private void Update()
    {
        // 쿨타임 감소
        if (_dashTimer > 0f) _dashTimer -= Time.deltaTime;

        // 대쉬 중이면 대쉬 지속, 아니면 일반 이동 + 대쉬 시도
        if (_isDashing)
        {
            ContinueDash();
        }
        else
        {
            Move();
            TryStartDash();
        }
    }

    public void Move()
    {
        if (CheckPlayerMove() == false) return;
        if (CheckPlayerAttribute() == false) return;
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        // 입력
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");

        // 방향 벡터
        Vector3 nextVector = _moveDirection.normalized * _playerAttribute.Attributes.UnitMoveSpeed * Time.deltaTime;

        // 이동
        this.transform.position = this.transform.position + nextVector;
    }

    private void TryStartDash()
    {
        // 쿨타임 / 모드 확인
        if (_dashTimer > 0f) return;
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        // 입력 체크
        if (!(Input.GetKeyDown(KeyCode.Space))) return;

        // 마우스 월드 좌표
        //var cam = Camera.main;
        //if (cam == null) return;
        //Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = transform.position.z;

        // 대쉬 방향 & 목표 설정
        _dashDir = _moveDirection.normalized;//(_moveDirection - transform.position).normalized;
        _dashTarget = transform.position + _dashDir * DashDistance;

        _isDashing = true;
        _dashTimer = DashCooldown;
    }

    private void ContinueDash()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        // 한 프레임 이동량
        Vector3 moveStep = _dashDir * DashSpeed * Time.deltaTime;
        Vector3 toTarget = _dashTarget - transform.position;

        // 목표 넘어서면 종료
        if (Vector3.Dot(toTarget, _dashDir) <= 0f || moveStep.sqrMagnitude >= toTarget.sqrMagnitude)
        {
            transform.position = _dashTarget;
            _isDashing = false;
        }
        else
        {
            transform.position += moveStep;
        }
    }
}
