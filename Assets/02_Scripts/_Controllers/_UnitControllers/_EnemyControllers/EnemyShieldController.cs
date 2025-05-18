using UnityEngine;

public class EnemyShieldController : MonoBehaviour
{
    [Tooltip("회전시킬 방패 Transform (자기 자신일 수도 있고, 자식일 수도 있습니다)")]
    [SerializeField] private GameObject _shield;

    [Tooltip("회전 속도 (도/초)")]
    [SerializeField] private float _rotationSpeed = 360f;

    [Tooltip("방패가 기본 바라보는 방향 오프셋 (Sprite가 0°일 때 전방을 가리키지 않으면 조정)")]
    [SerializeField] private float _angleOffset = 0f;

    private GameObject _player;

    private void Update()
    {
        if (CheckEnemyMoveTarget() == false) return;
        if (CheckShieldTarget() == false) return;

        EnemyShieldMove();
    }

    private bool CheckEnemyMoveTarget()
    {
        if (_player == null)
        {
            _player = AllManager.instance.GamemodeManager.Player;
            //Debug.LogError("Player 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckShieldTarget()
    {
        if (_shield == null)
        {
            _shield = this.transform.GetChild(0).gameObject; ;
            //Debug.LogError("Shield 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    private void EnemyShieldMove()
    {
        // 1) 플레이어 방향 계산
        Vector3 dir = (_player.transform.position - _shield.transform.position).normalized;

        // 2) 그 방향의 각도(도) 구하기
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + _angleOffset;

        // 3) 현재 회전과 목표 회전 사이를 부드럽게 보간
        Quaternion current = _shield.transform.rotation;
        Quaternion target = Quaternion.Euler(0f, 0f, targetAngle);
        _shield.transform.rotation = Quaternion.RotateTowards(current, target, _rotationSpeed * Time.deltaTime);
    }
}
