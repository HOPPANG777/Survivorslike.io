using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionUIController : MonoBehaviour
{
    [SerializeField] private Text _label;  // 무기 이름 표시용
    [SerializeField] private Image _icon;   // 무기 아이콘 표시용(필요 시)

    private System.Action _onClick;

    public void Init(System.Action onClick)
    {
        _onClick = onClick;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => _onClick());
    }

    /// <summary>
    /// 버튼의 표시 정보 설정
    /// </summary>
    public void SetData(WeaponType type)
    {
        if (_label != null) _label.text = type.ToString();
        // 아이콘을 사용한다면
        // if (_icon != null) _icon.sprite = ...;
    }
}
