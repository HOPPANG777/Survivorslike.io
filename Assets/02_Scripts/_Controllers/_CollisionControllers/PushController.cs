using UnityEngine;

public class PushController : ICollision
{
    public void OnCollision(ICollidable a, ICollidable b)
    {
        // 둘 다 IPushable이면 서로 밀치기
        if (a is IPushable pa && b is IPushable pb)
        {
            var dir = (b.Position - a.Position).normalized;
            pa.Push(-dir);
            pb.Push(dir);
        }
    }
}
