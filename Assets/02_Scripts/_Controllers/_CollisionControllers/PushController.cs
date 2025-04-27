using UnityEngine;

public class PushController : ICollision
{
    public void OnCollision(ICollidable a, ICollidable b)
    {
        if (a is IPushable pa && b is IPushable pb)
        {
            // 실제 Delta 벡터 (정규화 NO)
            Vector3 delta = b.Position - a.Position;
            pa.Push(-delta);
            pb.Push(delta);
        }
    }
}
