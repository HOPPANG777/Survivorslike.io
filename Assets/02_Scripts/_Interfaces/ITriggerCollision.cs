using UnityEngine;

// ITriggerCollision.cs
public interface ITriggerCollision
{
    /// <summary>트리거 충돌이 감지되었을 때 바로 호출</summary>
    void OnTriggerCollision(ICollidable self, ICollidable other);

    /// <summary>트리거 충돌 후 추가 후처리가 필요할 때 호출</summary>
    void OnPostTriggerCollision(ICollidable self, ICollidable other);
}

