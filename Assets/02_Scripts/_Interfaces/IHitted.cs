using UnityEngine;

public interface IHitted
{
    TeamType TeamType { get; set; }

    void Hitted(float damageValue);
}
