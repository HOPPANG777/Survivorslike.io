using System;

[Serializable]
public struct WeaponStatDiff
{
    public string Label;
    public float Value;

    public WeaponStatDiff(string label, float value)
    {
        Label = label;
        Value = value;
    }
}
