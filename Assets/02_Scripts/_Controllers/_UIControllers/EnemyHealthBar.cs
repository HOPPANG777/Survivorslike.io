using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Vector3 _offset = Vector3.up;

    private IPositionProvider _positionProvider;
    private IUnitAttribute<UnitAttribute> _attributeProvider;

    public void Bind(IPositionProvider positionProvider, IUnitAttribute<UnitAttribute> attributeProvider)
    {
        _positionProvider = positionProvider;
        _attributeProvider = attributeProvider;
    }

    private void LateUpdate()
    {
        if (_positionProvider == null || _attributeProvider == null) return;

        transform.position = _positionProvider.Position + _offset;

        UnitAttribute attr = _attributeProvider.Attributes;
        if (attr.UnitMaxHealth > 0f)
        {
            _fillImage.fillAmount = attr.UnitCurrentHealth / attr.UnitMaxHealth;
        }
    }
}
