using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyDashController : MonoBehaviour, IDashable, ISpeakable, ISetStatus
{
    [TabGroup("Target")][SerializeField] private GameObject _enemy;
    public GameObject Enemy { get { return _enemy; } }

    [TabGroup("Target")][Tooltip("타겟 포지션")][SerializeField] private IMoveable _enemyDirection;
    public IMoveable EnemyDirection { get { return _enemyDirection; } }

    [TabGroup("Dash")][Tooltip("대쉬 이동 거리(그리고 100 분율로 보십시오 따라서 50일 0.5f가 됩니다.)")][SerializeField] private int _dashDistance; // 5f;
    public float DashDistance { get { return _dashDistance / 100f; } set { _dashDistance = (int)value; } }

    [TabGroup("Dash")][Tooltip("대쉬 속도(그리고 100 분율로 보십시오 따라서 50일 0.5f가 됩니다.)")][SerializeField] private int _dashSpeed; // 20f;
    public float DashSpeed { get { return _dashSpeed / 100f; } set { _dashSpeed = (int)value; } }

    [TabGroup("Dash")][Tooltip("대쉬 쿨타임(s)(그리고 100 분율로 보십시오 따라서 50일 0.5f가 됩니다.)")][SerializeField] private int _dashCooldown; // 2f;
    public float DashCooldown { get { return _dashCooldown / 100f; } set { _dashCooldown = (int)value; } }

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

    // 내부 상태
    private bool _isDashing;
    private Vector3 _dashDir;
    private Vector3 _dashTarget;
    private float _dashTimer;

    [TabGroup("Dash AfterImage")][SerializeField] private float _afterImageInterval = 0.05f;
    [TabGroup("Dash AfterImage")][SerializeField] private float _afterImageDuration = 0.2f;
    private float _afterImageTimer;
    private SpriteRenderer _enemyRenderer;

    public void SetStatus()
    {
        if (_enemy == null)
        {
            _enemy = this.gameObject;
        }
        _enemyRenderer = _enemy.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Dash();
    }

    private bool CheckEnemyDash()
    {
        if (_enemy == null)
        {
            _enemy = this.gameObject;
            //Debug.LogError("Enemy가 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckEnemyDirection()
    {
        if (_enemy.TryGetComponent<IMoveable>(out var moveable))
        {
            _enemyDirection = moveable;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    public void Dash()
    {
        if (CheckEnemyDash() == false) return;
        if (CheckEnemyDirection() == false) return;

        // 쿨타임 감소
        if (_dashTimer > 0f) _dashTimer -= Time.deltaTime;

        // 대쉬 중이면 대쉬 지속, 아니면 일반 이동 + 대쉬 시도
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
        // 쿨타임 / 모드 확인
        if (_dashTimer > 0f) return;

        //SoundPlay();

        // 대쉬 방향 & 목표 설정
        _dashDir = _enemyDirection.MoveDirection.normalized; // (_moveDirection - transform.position).normalized;
        _dashTarget = this.transform.position + _dashDir * DashDistance;

        _isDashing = true;
        _dashTimer = DashCooldown;
    }

    public void ContinueDash()
    {
        // 한 프레임 이동량
        Vector3 moveStep = _dashDir * DashSpeed * Time.deltaTime;
        Vector3 toTarget = _dashTarget - transform.position;

        CreateAfterImage();

        // 목표 넘어서면 종료
        if (Vector3.Dot(toTarget, _dashDir) <= 0f || moveStep.sqrMagnitude >= toTarget.sqrMagnitude)
        {
            // SoundStop();
            this.transform.position = _dashTarget;
            _isDashing = false;
        }
        else
        {
            this.transform.position += moveStep;
        }
    }

    private void CreateAfterImage()
    {
        if (_enemyRenderer == null) return;
        _afterImageTimer -= Time.deltaTime;

        if (_afterImageTimer > 0f) return;
        _afterImageTimer = _afterImageInterval;

        var obj = new GameObject("DashAfterImage");
        obj.transform.position = this.transform.position;
        obj.transform.rotation = this.transform.rotation;
        var after = obj.AddComponent<DashAfterImage>();
        after.Initialize(_enemyRenderer.sprite, _enemyRenderer.color, _enemyRenderer.sortingOrder - 1, _afterImageDuration);
    }

    public void SoundPlay()
    {
        _soundSource.Play();
    }

    public void SoundStop()
    {
        _soundSource.Stop();
    }
}
