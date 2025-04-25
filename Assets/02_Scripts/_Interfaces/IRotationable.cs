using UnityEngine;

public interface IRotationable
{
    // 회전할 방향
    Vector3 RotationDirection { get; set; }

    // 회전
    public void Rotate();
}
