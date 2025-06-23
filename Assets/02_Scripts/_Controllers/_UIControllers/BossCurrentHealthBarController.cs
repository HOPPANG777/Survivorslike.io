using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BossCurrentHealthBarController : MonoBehaviour
{
    [TabGroup("Image")][Tooltip("보스 체력 바 이미지")][SerializeField]
    private Image _bossCurrentHealthBar;
    public Image BossCurrentHealthBar { get { return _bossCurrentHealthBar; } }

    private IUnitAttribute<UnitAttribute> _bossAttribute;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Bind(IUnitAttribute<UnitAttribute> attribute)
    {
        _bossAttribute = attribute;
        if (_bossAttribute != null)
        {
            gameObject.SetActive(true);
            UpdateBar();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_bossAttribute == null) return;
        UpdateBar();
    }

    private void UpdateBar()
    {
        UnitAttribute attr = _bossAttribute.Attributes;
        if (attr.UnitMaxHealth > 0f)
        {
            _bossCurrentHealthBar.fillAmount = attr.UnitCurrentHealth / attr.UnitMaxHealth;
        }
    }

    public void ResetBar()
    {
        _bossAttribute = null;
        if (_bossCurrentHealthBar != null) _bossCurrentHealthBar.fillAmount = 1f;
        gameObject.SetActive(false);
    }
}
