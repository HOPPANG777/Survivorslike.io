using UnityEngine;

public interface IItemFactory
{
    ItemBase Create(ItemBaseSO data, Vector3 position);
    void Release(ItemBase item);
    void ReleaseAll();
}
