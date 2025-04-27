using UnityEngine;

public class PushController : ICollision
{
    public void OnCollision(ICollidable a, ICollidable b)
    {
        if (a is IPushable pa && b is IPushable pb)
        {
            var dir = (b.Position - a.Position).normalized;
            pa.Push(-dir);
            pb.Push(dir);
        }
    }
}
