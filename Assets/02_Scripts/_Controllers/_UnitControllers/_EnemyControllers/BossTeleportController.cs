using UnityEngine;
using Sirenix.OdinInspector;

//==============================================================================
// BossTeleportController
//==============================================================================
// 보스 전용 텔레포트 컨트롤러. 카메라 주변 네 지점 중 한 곳으로 주기적으로 이동한다.
//==============================================================================
public class BossTeleportController : MonoBehaviour, ITeleport
{
    [TabGroup("Target")][SerializeField] private GameObject _boss;
    public GameObject Boss { get { return _boss; } }

    [TabGroup("Target")][SerializeField] private IPositionProvider _bossPosition;
    public IPositionProvider BossPosition { get { return _bossPosition; } }

    [TabGroup("Teleport")][SerializeField] private float _teleportInterval = 5f;
    public float TeleportInterval { get { return _teleportInterval; } set { _teleportInterval = value; } }

    [TabGroup("Teleport")][SerializeField][Range(0,100)] private int _teleportMarginPercent = 10;
    public int TeleportMarginPercent { get { return _teleportMarginPercent; } set { _teleportMarginPercent = value; } }

    private Camera _mainCamera;
    private float _timer;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Teleport();
    }

    private bool CheckBoss()
    {
        if (_boss == null)
        {
            _boss = this.gameObject;
            return false;
        }
        return true;
    }

    private bool CheckPosition()
    {
        if (_boss.TryGetComponent<IPositionProvider>(out var pos))
        {
            _bossPosition = pos;
            return true;
        }
        return false;
    }

    public void Teleport()
    {
        if (CheckBoss() == false) return;
        if (CheckPosition() == false) return;

        _timer += Time.deltaTime;
        if (_timer >= TeleportInterval)
        {
            _timer -= TeleportInterval;
            TeleportBoss();
        }
    }

    // 실제 텔레포트 처리
    private void TeleportBoss()
    {
        if (_mainCamera == null) return;
        float height = 2f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;
        Vector3 center = _mainCamera.transform.position;

        float xOff = width / 2f - width * TeleportMarginPercent / 100f;
        float yOff = height / 2f - height * TeleportMarginPercent / 100f;

        Vector3[] pos = new Vector3[4];
        pos[0] = new Vector3(center.x - xOff, center.y, _bossPosition.Position.z); // 왼쪽
        pos[1] = new Vector3(center.x + xOff, center.y, _bossPosition.Position.z); // 오른쪽
        pos[2] = new Vector3(center.x, center.y + yOff, _bossPosition.Position.z); // 위
        pos[3] = new Vector3(center.x, center.y - yOff, _bossPosition.Position.z); // 아래

        int idx = Random.Range(0, pos.Length);
        _boss.transform.position = pos[idx];
    }
}
