using UnityEngine;

public class HpItem : ItemBase
{
    [Header("HP Item")]
    private HpSO _hpData;
    public HpSO HpSO { get { return _hpData; } set { _hpData = value; } }

    protected override float CurrentAttractRange
    {
        get
        {
            var player = AllManager.instance.GamemodeManager.Player.GetComponent<PlayerUnit>();
            if (player != null) return player.Attributes.PlayerGetExpRange;
            return base.CurrentAttractRange;
        }
    }

    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        base.Update();
    }

    public override void SetStatus()
    {
        base.SetStatus();
        _hpData = ItemData as HpSO;
    }

    protected override void Collect()
    {
        base.Collect();
    }

    public override void CheckDead()
    {
        base.CheckDead();
    }

    public override void Dead()
    {
        base.Dead();
    }

    protected override void PlayerGetItem()
    {
        base.PlayerGetItem();
    }
}
