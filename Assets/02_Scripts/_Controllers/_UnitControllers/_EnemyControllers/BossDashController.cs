using UnityEngine;
using Sirenix.OdinInspector;

//==============================================================================
// BossDashController
//==============================================================================
// 보스 전용 대쉬 컨트롤러. 연속 대쉬를 지원한다.
//==============================================================================
public class BossDashController : MonoBehaviour, IDashable, ISetStatus, ISpeakable
{
    [TabGroup("Target")][SerializeField] private GameObject _boss;
    public GameObject Boss { get { return _boss; } }

    [TabGroup("Target")][SerializeField] private IMoveable _bossDirection;
    public IMoveable BossDirection { get { return _bossDirection; } }

    [TabGroup("Dash")][SerializeField] private float _dashDistance;
    public float DashDistance { get { return _dashDistance; } set { _dashDistance = value; } }

    [TabGroup("Dash")][SerializeField] private float _dashSpeed;
    public float DashSpeed { get { return _dashSpeed; } set { _dashSpeed = value; } }

    [TabGroup("Dash")][SerializeField] private float _dashCooldown;
    public float DashCooldown { get { return _dashCooldown; } set { _dashCooldown = value; } }

    // 연속 대쉬 횟수
    [TabGroup("Dash")][SerializeField] private int _consecutiveDash = 3;
    public int ConsecutiveDash { get { return _consecutiveDash; } set { _consecutiveDash = value; } }

    // 사용할 사운드 이름
    [TabGroup("Dash Sound")][Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [TabGroup("Dash Sound")][Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [TabGroup("Dash Sound")][Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    // 오디오를 출력할 주체
    [TabGroup("Dash Sound")][Tooltip("오디오를 출력할 주체")][SerializeField] protected AudioSource _soundSource;
    public AudioSource SoundSource { get { return _soundSource; } }

    private bool _isDashing;
    private Vector3 _dashDir;
    private Vector3 _dashTarget;
    private float _dashTimer;
    private int _remainDash;

    [TabGroup("Dash AfterImage")][SerializeField] private float _afterImageInterval = 0.05f;
    [TabGroup("Dash AfterImage")][SerializeField] private float _afterImageDuration = 1.0f;
    private float _afterImageTimer;
    private SpriteRenderer _enemyRenderer;
    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Dash();
    }

    public void SetStatus()
    {
        if (_boss == null)
        {
            _boss = this.gameObject;
        }
    }

    public void SoundPlay()
    {

    }

    public void SoundStop()
    {

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

        CreateAfterImage();

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

    private void CreateAfterImage()
    {
        if (_enemyRenderer == null) return;
        _afterImageTimer -= Time.deltaTime;

        if (_afterImageTimer > 0f) return;
        _afterImageTimer = _afterImageInterval;

        var dashEffect = AllManager.instance.ParticleFactory.Create(ParticleType.PARTICLE_DASH, this.transform.position, Quaternion.identity);
        var after = dashEffect.GetComponent<DashAfterImage>();
        after.Initialize(_enemyRenderer.sprite, _enemyRenderer.color, _enemyRenderer.sortingOrder - 1, _afterImageDuration, this.gameObject.GetComponent<SpriteRenderer>().size, this.transform.rotation);
    }
}
