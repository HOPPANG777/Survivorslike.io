using UnityEngine;

public interface ICollision
{
    void OnCollision(ICollidable self, ICollidable other);
}
