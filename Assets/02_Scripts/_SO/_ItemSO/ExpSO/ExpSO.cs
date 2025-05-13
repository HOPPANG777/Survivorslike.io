using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpGem", menuName = "ItemData/ExpGemSO", order = 1)]
public class ExpGemSO : ItemBaseSO
{
    [TabGroup("Exp Gem Data")][Tooltip("경험치 양")][SerializeField] private int _expAmount;
    public int ExpAmount { get { return _expAmount; } set { _expAmount = value; } }

    public override void Apply(GameObject user)
    {
        var player = user.GetComponent<PlayerGetExpController>();
        if (player != null) player.AddExperience(_expAmount);
    }
}
