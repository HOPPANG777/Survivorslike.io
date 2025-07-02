using UnityEngine;

public class HittedController : ICollision
{
    public void OnCollision(ICollidable self, ICollidable other)
    {
        // self가 맞는 쪽이고, other가 때리는 쪽이면
        if (self is IHitted target && other is IAttacker attacker)
        {
            if (target.TeamType == attacker.TeamType) return; // 같은 팀이면 충돌 무시

            // 피격 방향 계산
            Vector3 dir = self.Position - other.Position;

            // 피격 처리: IHitted 인터페이스를 구현한 객체에 피격 처리
            target.Hitted(attacker.AttackPower);

            // 2) Projectile 처리: LaserWeaponProjectile은 적과 충돌해도 카운트를 감소시키지 않는다
            if (attacker is LaserWeaponProjectile laserProj)
            {
                laserProj.CheckDead();
                SoundManager.instance.PlaySound(laserProj.SoundName, laserProj);
            }
            else if (attacker is ProjectileBase proj)
            {
                switch (proj.DisableType)
                {
                    case WeaponDisableType.WEAPONDISABLETYPE_NONE:
                        {
                            proj.RemainingPierceCount--;
                            proj.CheckDead();
                        }
                        break;
                    case WeaponDisableType.WEAPONDISABLETYPE_COUNT:
                        {
                            proj.RemainingPierceCount--;
                            proj.CheckDead();
                        }
                        break;
                    default:
                        break;
                }
                SoundManager.instance.PlaySound(proj.SoundName, proj);
            }
        }
    }
}
