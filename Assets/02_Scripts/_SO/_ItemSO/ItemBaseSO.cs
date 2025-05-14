using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/ItemBaseSO", order = 0)]
public abstract class ItemBaseSO : ScriptableObject
{
    [TabGroup("Item Data")][Tooltip("아이템 이름")][SerializeField] private string _itemName;
    public string ItemName { get { return _itemName; } set { _itemName = value; } }

    [TabGroup("Item Data")][Tooltip("아이템 아이콘")][SerializeField] private Sprite _icon;
    public Sprite Icon { get { return _icon; } set { _icon = value; } }

    [TabGroup("Item Data")][Tooltip("충돌 반지름")][SerializeField] private float _collisionRadius;
    public float CollisionRadius { get { return _collisionRadius; } set { _collisionRadius = value; } }

    [TabGroup("Item Data")][Tooltip("플레이어 끌어당김 범위")][SerializeField] private float _attractRange;
    public float AttractRange { get { return _attractRange; } set { _attractRange = value; } }

    [TabGroup("Item Data")][Tooltip("끌어당김 속도")][SerializeField] private float _attractSpeed;
    public float AttractSpeed { get { return _attractSpeed; } set { _attractSpeed = value; } }

    public abstract void Apply(GameObject user);

    public virtual void SetData(ItemBaseSO data)
    {
        this.ItemName = data.ItemName;

        this.Icon = data.Icon;

        this.CollisionRadius = data.CollisionRadius;

        this.AttractRange = data.AttractRange;

        this.AttractRange = data.AttractRange;

    }
}
