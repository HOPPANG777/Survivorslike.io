using UnityEngine;

public interface ICameraBoundDetectable
{
    // 프레임마다 카메라 밖인지 갱신
    void UpdateCameraBound();
    // 카메라 밖으로 처음 나갈 때
    void OnExitCamera();
    // 카메라 안으로 처음 들어올 때
    void OnEnterCamera();
    // 현재 카메라 안/밖 상태 제공(선택)
    bool IsOutOfCamera { get; }
}