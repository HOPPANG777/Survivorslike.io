using UnityEngine;

public class MagnetItem : ItemBase
{
    [Header("Magnet Item")]
    private MagnetSO _magnetData;
    public MagnetSO MagnetSO { get { return _magnetData; } set { _magnetData = value; } }

    public override void SetStatus()
    {
        base.SetStatus();
        _magnetData = ItemData as MagnetSO;
    }
}

