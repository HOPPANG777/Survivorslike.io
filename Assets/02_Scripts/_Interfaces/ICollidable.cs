using UnityEngine;

public interface ICollidable : IPositionProvider
{
    float CollisionRadius { get; }
}
