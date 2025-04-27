using UnityEngine;

public interface IAttacker
{
    TeamType TeamType { get; set; }

    float AttackPower { get; }
}
