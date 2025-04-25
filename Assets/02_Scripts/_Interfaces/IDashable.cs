using UnityEngine;

public interface IDashable
{
    // 대쉬 할 거리
    public float DashDistance { get; set;}
    
    // 대쉬 스피드
    public float DashSpeed { get; set; }

    // 대쉬 쿨다운
    public float DashCooldown { get; set; }

    // 객체의 대쉬
    public void Dash();
}
