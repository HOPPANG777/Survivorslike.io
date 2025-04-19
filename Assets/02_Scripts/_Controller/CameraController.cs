using UnityEngine;

//==============================================================================
// CameraController
//==============================================================================
// Camera의 이동을 담당하는 Controller
//
//==============================================================================
public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("카메라")][SerializeField] private Camera _camera;
    public Camera MainCamera { get { return _camera; } }

    [Tooltip("카메라가 따라다니는 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _cameraFollowSpeed; // 기본이 4
    public float CameraFollowSpeed { get {  return _cameraFollowSpeed / 100; } }

    [Tooltip("카메라가 따라다니는 속도(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _cameraFixedZ; // 기본이 10
    public float CameraFixedZ { get { return _cameraFixedZ / 100; } }

    [Header("Target")]
    [SerializeField] private GameObject _cameraTarget;
    public GameObject CameraTarget { get { return _cameraTarget; } }

    [SerializeField] private IPositionProvider _cameraTargetPosition;
    public IPositionProvider CameraTargetPosition { get { return _cameraTargetPosition; } }

    private bool CheckCamera()
    {
        if (_cameraTarget.TryGetComponent<IPositionProvider>(out var provider))
        {
            _cameraTargetPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    public void FollowCameraMove()
    {
        if (CheckCamera() == false) return;

        // 카메라가 따라가는 로직
        MainCamera.transform.position = Vector2.Lerp
        (
            MainCamera.transform.position,
            _cameraTargetPosition.Position,
            CameraFollowSpeed * Time.deltaTime
        );
        MainCamera.transform.Translate(0, 0, CameraFixedZ);
    }
}
