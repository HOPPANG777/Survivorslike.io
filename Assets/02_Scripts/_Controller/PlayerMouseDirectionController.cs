using UnityEngine;

//==============================================================================
// PlayerMouseDirectionController
//==============================================================================
// Player의 회전을 담당하는 Controller
//
//==============================================================================
public class PlayerMouseDirectionController : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("타겟")][SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } }

    [Tooltip("타겟 포지션")][SerializeField] private IPositionProvider _playerPosition;
    public IPositionProvider PlayerPosition { get { return _playerPosition; } }

    [Header("PlayerMouseDirection")]
    [Tooltip("회전 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _rotationSpeed; // 0이면 즉시회전, >0이면 부드럽게 회전
    public float RotationSpeed { get { return _rotationSpeed / 100f; } }

    private bool CheckPlayerMouseDirection()
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

    private bool CheckTargetPosition()
    {
        if (_player.TryGetComponent<IPositionProvider>(out var provider))
        {
            _playerPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    private void Update()
    {
        FaceMousePointer();
    }

    private void FaceMousePointer()
    {
        if (CheckPlayerMouseDirection() == false) return;
        if (CheckTargetPosition() == false) return;

        // 1) 마우스 스크린 좌표
        Vector3 mouseScreenPos = Input.mousePosition;

        // 2) 카메라로부터의 z 거리를 설정 (Orthographic이면 z 값은 크게 상관없음)
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z - _playerPosition.Position.z);

        // 3) 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // 4) 플레이어→마우스 방향 벡터
        Vector2 dir = (mouseWorldPos - _playerPosition.Position).normalized;

        // 5) Atan2로 각도 계산 (도 단위)
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // 기본 스프라이트 방향(오른쪽)이 0°일 때 맞춰짐. 
        // 만약 위쪽이 0°라면 targetAngle -= 90f 해 주세요.

        // 즉시 회전
        if (RotationSpeed <= 0f)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }
        else // 부드러운 회전
        {
            Quaternion tgtRot = Quaternion.Euler(0f, 0f, targetAngle);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, tgtRot, RotationSpeed * Time.deltaTime);
        }
    }
}
