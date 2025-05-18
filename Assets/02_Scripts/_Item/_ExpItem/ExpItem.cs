using UnityEngine;

public class ExpItem : ItemBase
{
    [Header("Exp Item")]
    private ExpSO _expData;
    public ExpSO ExpSO { get { return _expData; } set { _expData = value; } }

    protected override float CurrentAttractRange
    { 
        get 
        {  
            var player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerUnit>();
            if (player != null) return player.Attributes.PlayerGetExpRange;
            return base.CurrentAttractRange;
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetStatus()
    {
        base.SetStatus();
    }

    protected override void Collect()
    {
        base.Collect();
        _expData.Apply(Player.gameObject);
    }

    protected override void OnUse()
    {
        base.OnUse();
    }

    protected override void PlayerGetItem()
    {
        base.PlayerGetItem();
    }

}
