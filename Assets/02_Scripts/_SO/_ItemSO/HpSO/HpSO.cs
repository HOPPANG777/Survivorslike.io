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
            float healAmount = attr.UnitMaxHealth * _healRate;
            float prevHealth = attr.UnitCurrentHealth;

            attr.UnitCurrentHealth = Mathf.Min(attr.UnitCurrentHealth + healAmount, attr.UnitMaxHealth);
            healAmount = attr.UnitCurrentHealth - prevHealth;

            attrProvider.Attributes = attr;
            AllManager.instance.UIManager.PlayerCurrentHealthBarController.UpdatePlayerHealthBarImage(attr.UnitCurrentHealth / attr.UnitMaxHealth);

            var parent = AllManager.instance.UIPoolCanvas != null ? AllManager.instance.UIPoolCanvas.CanvasRoot : null;
            var go = AllManager.instance.UIFactory.Create(UIType.UI_DAMAGE_TEXT, parent);

            if (go != null)
            {
                var dmg = go.GetComponent<DamageTextController>();
                if (dmg != null && healAmount > 0f)
                {
                    dmg.Play(healAmount, user.transform.position, Color.green);
                }
            }
        }
    }
}
