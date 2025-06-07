using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerUnit))]
public class PlayerLevelUpController : MonoBehaviour
{
    [Header("Leveling Formula")]
    [Tooltip("레벨1 → 2에 필요한 EXP(젬 개수)")]
    [SerializeField] private int _baseExpRequirement = 5;
    [Tooltip("레벨업마다 필요 EXP에 곱할 배율")]
    [SerializeField] private float _expGrowthRate = 1.3f;

    private IUnitAttribute<UnitAttribute> _unitAttr;
    private IUnitAttribute<PlayerableAttribute> _playerAttr;
    private PlayerCurrentExpBarController _expBar;
    private PlayerCurrentLevelCountController _levelCounter;

    private void Awake()
    {
        _unitAttr = GetComponent<IUnitAttribute<UnitAttribute>>();
        _playerAttr = GetComponent<IUnitAttribute<PlayerableAttribute>>();
        _expBar = AllManager.instance.UIManager.PlayerCurrentExpBarController;
        _levelCounter = AllManager.instance.UIManager.PlayerCurrentLevelCountController;
        UpdateUI();
    }

    /// <summary>
    /// 남은 EXP를 그대로 유지한 채로 레벨업을 시도합니다.
    /// </summary>
    public void TryLevelUp()
    {
        // ① struct 복사
        var pAttr = _playerAttr.Attributes;
        var uAttr = _unitAttr.Attributes;

        int curLevel = pAttr.PlayerLevel;
        float curExp = pAttr.PlayerCurrentExp;

        // ② 필요 EXP 계산
        int needed = GetExpForLevel(curLevel);

        // ③ 레벨업 가능할 때까지 반복
        while (curExp >= needed)
        {
            curExp -= needed;
            curLevel++;

            // 레벨업 이펙트/UI
            AllManager.instance.UIManager.LevelUpPage();

            // 다음 레벨 필요 EXP 갱신
            needed = GetExpForLevel(curLevel);
        }

        // ⑤ struct 수정값 반영
        pAttr.PlayerLevel = curLevel;
        pAttr.PlayerCurrentExp = curExp;
        _playerAttr.Attributes = pAttr;

        _unitAttr.Attributes = uAttr;

        // ⑥ UI 갱신
        UpdateUI();
    }

    /// <summary>레벨ℓ → ℓ+1에 필요한 EXP</summary>
    private int GetExpForLevel(int level)
    {
        float req = _baseExpRequirement * Mathf.Pow(_expGrowthRate, level - 1);
        return Mathf.CeilToInt(req);
    }

    /// <summary>EXP 바와 레벨 텍스트 동기화</summary>
    private void UpdateUI()
    {
        var pAttr = _playerAttr.Attributes;
        int level = pAttr.PlayerLevel;
        int needed = GetExpForLevel(level);
        float fill = needed > 0 ? pAttr.PlayerCurrentExp / (float)needed : 0f;

        _expBar.UpdatePlayerCurrentExpBar(fill);
        _levelCounter.UpdateLevelCounter(level);
    }
}
