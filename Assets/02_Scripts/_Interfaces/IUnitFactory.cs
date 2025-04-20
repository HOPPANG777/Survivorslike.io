using UnityEngine;

public interface IUnitFactory
{
    UnitBase Create(UnitBaseSO data, Vector3 position);

    void Release(UnitBase unit);
}
