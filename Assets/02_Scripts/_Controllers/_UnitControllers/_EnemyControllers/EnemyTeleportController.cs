using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTeleportController : MonoBehaviour, ITeleport
{
    [TabGroup("Target")][SerializeField] private GameObject _enemy;
    public GameObject Enemy { get { return _enemy; } }

    [TabGroup("Target")][Tooltip("타겟 포지션")][SerializeField] private IPositionProvider _enemyPosition;
    public IPositionProvider EnemyPosition { get { return _enemyPosition; } }

    [TabGroup("Teleport")][Tooltip("적을 텔레포트할 간격 (초)")][SerializeField] private float _teleportInterval;
    public float TeleportInterval { get { return _teleportInterval; } set { _teleportInterval = value; } }

    [TabGroup("Teleport")][Tooltip("카메라 뷰 내에서 텔레포트 시킬 위치의 마진(%)")][SerializeField][Range(0, 100)] private int _teleportMarginPercent;
    public int TeleportMarginPercent { get { return _teleportMarginPercent; } set { _teleportMarginPercent = value; } }

    private Camera _mainCamera;
    private float _timer = 0f;

    private bool CheckEnemyMoveTarget()
    {
        if (_enemy == null)
        {
            _enemy = this.gameObject;
            //Debug.LogError("Enemy 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckTargetPosition()
    {
        if (_enemy.TryGetComponent<IPositionProvider>(out var provider))
        {
            _enemyPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Teleport();
    }

    public void Teleport()
    {
        _timer += Time.deltaTime;
        if (_timer >= TeleportInterval)
        {
            // 인터벌만큼 빼고 Teleport 호출
            _timer -= TeleportInterval;
            TeleportEnemy(_enemyPosition.Position);
        }
    }

    private void TeleportEnemy(Vector3 enemy)
    {
        if (CheckEnemyMoveTarget() == false) return;
        if (CheckTargetPosition() == false) return;
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        // 카메라 절반 높이 & 너비
        float height = 2f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;
        Vector3 center = _mainCamera.transform.position;

        // 마진만큼 영역 축소
        float xMin = center.x - width / 2f + width * TeleportMarginPercent;
        float xMax = center.x + width / 2f - width * TeleportMarginPercent;
        float yMin = center.y - height / 2f + height * TeleportMarginPercent;
        float yMax = center.y + height / 2f - height * TeleportMarginPercent;

        // 랜덤 좌표
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        // 순간이동
        enemy = new Vector3(randomX, randomY, enemy.z);
    }
}

