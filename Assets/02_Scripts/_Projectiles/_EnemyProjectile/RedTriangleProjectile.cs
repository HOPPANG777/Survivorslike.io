using UnityEngine;

public class RedTriangleProjectile : ProjectileBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetStatus()
    {
        base.SetStatus();
    } 

    protected override void Move()
    {
        base.Move();
    }

    protected override void AgeCheck()
    {
        base.AgeCheck();
    }

    public override void Dead()
    {
        base.Dead();
    }

    public override void OnPostCollision(ICollidable other)
    {
        base.OnPostCollision(other);
    }
}

//// 예시로 폭발형 투사체라면
//public override void OnPostCollision(ICollidable other)
//{
//    // 폭발 이펙트 생성
//    Explode();
//    // 부모 동작(반환 등)은 기본 Update에서 처리됨
//}

//private void Explode()
//{
//    // 폭발 범위 안의 ICollidable들을 찾아서 데미지 주기
//    var hits = Physics2D.OverlapCircleAll(transform.position, Attributes.ExplosionRadius);
//    foreach (var col in hits)
//    {
//        if (col.TryGetComponent<ICollidable>(out var c))
//        {
//            // HittedController가 붙은 대상을 IAttacker로 처리
//            AllManager.instance.CollisionManager.HandleSingleCollision(this, c);
//        }
//    }
//}