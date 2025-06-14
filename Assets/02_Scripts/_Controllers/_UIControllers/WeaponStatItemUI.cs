using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatItemUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _valueLabel;

    public void SetData(Sprite icon, float value)
    {
        if (_icon != null)
        {
            _icon.sprite = icon;
            _icon.enabled = icon != null;
        }

        if (_valueLabel != null)
        {
            string sign = value > 0 ? "+" : string.Empty;
            _valueLabel.text = $"{sign}{value:0.#}";
        }
    }
}
