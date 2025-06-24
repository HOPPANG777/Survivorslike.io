using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 무기 정보를 마우스 포인터 주변에 표시해 주는 컨트롤러
/// </summary>
public class WeaponTooltipController : MonoBehaviour
{
    [Tooltip("표시할 카드 프리팹")]
    [SerializeField] private WeaponSelectionUIController _cardPrefab;

    private WeaponSelectionUIController _cardInstance;

    private void EnsureCardSetup()
    {
        if (_cardInstance == null) return;
        var cg = _cardInstance.GetComponent<CanvasGroup>();
        if (cg == null) cg = _cardInstance.gameObject.AddComponent<CanvasGroup>();
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    /// <summary>
    /// 지정된 무기의 스탯을 카드에 표시하고 화면에 띄웁니다.
    /// </summary>
    /// <param name="weapon">표시할 무기</param>
    /// <param name="screenPos">마우스 위치(스크린 좌표)</param>
    public void ShowTooltip(WeaponBase weapon, Vector2 screenPos)
    {
        if (weapon == null || _cardPrefab == null) return;

        if (_cardInstance == null)
        {
            _cardInstance = Instantiate(_cardPrefab, transform);
            EnsureCardSetup();
        }
        else
        {
            EnsureCardSetup();
        }

        _cardInstance.gameObject.SetActive(true);
        _cardInstance.SetData(weapon.WeaponType, weapon.WeaponName, BuildStatList(weapon.Attributes));

        Vector3 pos = screenPos;
        pos.y -= 30f; // 마우스 아래쪽에 배치

        var rect = _cardInstance.GetComponent<RectTransform>();
        float halfW = rect.rect.width * rect.lossyScale.x * 0.5f;
        float halfH = rect.rect.height * rect.lossyScale.y * 0.5f;
        pos.x = Mathf.Clamp(pos.x, halfW, Screen.width - halfW);
        pos.y = Mathf.Clamp(pos.y, halfH, Screen.height - halfH);

        rect.position = pos;
    }

    /// <summary>
    /// 카드 숨기기
    /// </summary>
    public void HideTooltip()
    {
        if (_cardInstance != null)
        {
            _cardInstance.gameObject.SetActive(false);
        }
    }

    // WeaponAttribute를 WeaponStatDiff 리스트로 변환
    private List<WeaponStatDiff> BuildStatList(WeaponAttribute attr)
    {
        var list = new List<WeaponStatDiff>();
        void AddFloat(string label, float val)
        {
            if (Mathf.Approximately(val, 0f)) return;
            list.Add(new WeaponStatDiff(label, val));
        }
        void AddInt(string label, int val)
        {
            if (val == 0) return;
            list.Add(new WeaponStatDiff(label, val));
        }
        AddFloat("Damage", attr.FlatDamage);
        AddFloat("Dmg%", attr.DamageMultiplier);
        AddFloat("Crit", attr.CriticalDamage);
        AddFloat("Crit%", attr.CriticalChance);
        AddFloat("AS", attr.AttackSpeedMultiplier);
        AddInt("Count", attr.ProjectileCountBonus);
        AddFloat("Range", attr.RangeBonus);
        AddFloat("CD", attr.AttackCooldown);
        AddFloat("Knock", attr.KnockbackForce);
        AddInt("Pierce", attr.WeaponDisableCount);
        AddFloat("Time", attr.WeaponDisableTime);
        return list;
    }
}
