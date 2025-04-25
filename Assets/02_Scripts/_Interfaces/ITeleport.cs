using UnityEngine;

public interface ITeleport
{

    float TeleportInterval { get; set; }

    int TeleportMarginPercent { get; set; }

    void Teleport();
}
