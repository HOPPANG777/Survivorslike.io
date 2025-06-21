using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "HpItem", menuName = "ItemData/HpItemSO", order = 2)]
public class HpSO : ItemBaseSO
{
    [TabGroup("Hp Item Data")]
    [Tooltip("플레이어 체력 회복 비율")]
    [SerializeField] private float _healRate = 0.3f;
    public float HealRate { get { return _healRate; } set { _healRate = value; } }

    public override void Apply(GameObject user)
    {
        var attrProvider = user.GetComponent<IUnitAttribute<UnitAttribute>>();
        if (attrProvider != null)
        {
            var attr = attrProvider.Attributes;
            attr.UnitCurrentHealth += attr.UnitMaxHealth * _healRate;
            if (attr.UnitCurrentHealth > attr.UnitMaxHealth)
            {
                attr.UnitCurrentHealth = attr.UnitMaxHealth;
            }
            attrProvider.Attributes = attr;
            AllManager.instance.UIManager.PlayerCurrentHealthBarController.UpdatePlayerHealthBarImage(attr.UnitCurrentHealth / attr.UnitMaxHealth);
        }
    }
}
