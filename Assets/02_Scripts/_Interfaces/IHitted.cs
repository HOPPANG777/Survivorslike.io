using UnityEngine;

public interface IHitted
{
    TeamType TeamType { get; set; }
    // 현재 무적 상태인지 여부
    bool IsInvincible { get; }

    void Hitted(float damageValue);
}
