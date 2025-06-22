using UnityEngine;
using Sirenix.OdinInspector;

//==============================================================================
// BossDashController
//==============================================================================
// 보스 전용 대쉬 컨트롤러. 연속 대쉬를 지원한다.
//==============================================================================
public class BossDashController : MonoBehaviour, IDashable, ISetStatus
{
    [TabGroup("Target")][SerializeField] private GameObject _boss;
    public GameObject Boss { get { return _boss; } }

    [TabGroup("Target")][SerializeField] private IMoveable _bossDirection;
    public IMoveable BossDirection { get { return _bossDirection; } }

    [TabGroup("Dash")][SerializeField] private int _dashDistance;
    public float DashDistance { get { return _dashDistance / 100f; } set { _dashDistance = (int)value; } }

    [TabGroup("Dash")][SerializeField] private int _dashSpeed;
    public float DashSpeed { get { return _dashSpeed / 100f; } set { _dashSpeed = (int)value; } }

    [TabGroup("Dash")][SerializeField] private int _dashCooldown;
    public float DashCooldown { get { return _dashCooldown / 100f; } set { _dashCooldown = (int)value; } }

    // 연속 대쉬 횟수
    [TabGroup("Dash")][SerializeField] private int _consecutiveDash = 3;
    public int ConsecutiveDash { get { return _consecutiveDash; } set { _consecutiveDash = value; } }

    private bool _isDashing;
    private Vector3 _dashDir;
    private Vector3 _dashTarget;
    private float _dashTimer;
    private int _remainDash;

    public void SetStatus()
    {
        if (_boss == null)
        {
            _boss = this.gameObject;
        }
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Dash();
    }

    private bool CheckBossDash()
    {
        if (_boss == null)
        {
            _boss = this.gameObject;
            return false;
        }
        return true;
    }

    private bool CheckBossDirection()
    {
        if (_boss.TryGetComponent<IMoveable>(out var move))
        {
            _bossDirection = move;
            return true;
        }
        return false;
    }

    public void Dash()
    {
        if (CheckBossDash() == false) return;
        if (CheckBossDirection() == false) return;

        if (_dashTimer > 0f) _dashTimer -= Time.deltaTime;

        if (_isDashing)
        {
            ContinueDash();
        }
        else
        {
            TryStartDash();
        }
    }

    public void TryStartDash()
    {
        if (_dashTimer > 0f) return;

        _dashDir = _bossDirection.MoveDirection.normalized;
        _dashTarget = transform.position + _dashDir * DashDistance;

        _isDashing = true;
        _remainDash = ConsecutiveDash;
        _dashTimer = DashCooldown;
    }

    public void ContinueDash()
    {
        Vector3 moveStep = _dashDir * DashSpeed * Time.deltaTime;
        Vector3 toTarget = _dashTarget - transform.position;

        if (Vector3.Dot(toTarget, _dashDir) <= 0f || moveStep.sqrMagnitude >= toTarget.sqrMagnitude)
        {
            transform.position = _dashTarget;
            _remainDash--;
            if (_remainDash > 0)
            {
                _dashDir = _bossDirection.MoveDirection.normalized;
                _dashTarget = transform.position + _dashDir * DashDistance;
            }
            else
            {
                _isDashing = false;
            }
        }
        else
        {
            transform.position += moveStep;
        }
    }
}
