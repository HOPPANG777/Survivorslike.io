using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HittedController : ICollision
{
    public void OnCollision(ICollidable self, ICollidable other)
    {
        // self가 맞는 쪽이고, other가 때리는 쪽이면
        if (self is IHitted target && other is IAttacker attacker)
        {
            if (target.TeamType == attacker.TeamType) return; // 같은 팀이면 충돌 무시

            target.Hitted(attacker.AttackPower);
        }
    }
}
