using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionController : MonoBehaviour, ISelectedIndex
{
    [TabGroup("UI")]
    [Tooltip("무기 선택 패널(버튼 3개 포함)")]
    [SerializeField] private GameObject _itemPanel;

    [TabGroup("UI")]
    [Tooltip("버튼 3개 (Left→Right 순)")]
    [SerializeField] private List<WeaponSelectionUIController> _choiceButtons; // 버튼 컴포넌트

    [Header("Dependencies")]
    [SerializeField] private MonoBehaviour _playerHolderObj;  // IPlayerHolder 구현체 (PlayerUnit)
    [SerializeField] private MonoBehaviour _weaponFactoryObj; // IWeaponFactory 구현체

    private IPlayerHolder _playerHolder;
    private IWeaponFactory _weaponFactory;

    // 이번 레벨업에 뽑힌 3개의 후보
    private WeaponType[] _currentChoices = new WeaponType[0];

    // 고정된 12개 무기 타입 목록
    //private static readonly WeaponType[] _allWeapons =
    //{
    //    WeaponType.WEAPON_SNIPER,
    //    WeaponType.WEAPON_PISTOL,
    //    WeaponType.WEAPON_GATLING,
    //    WeaponType.WEAPON_MISSILE,
    //    WeaponType.WEAPON_AEGIS,
    //    WeaponType.WEAPON_BOMB,
    //    WeaponType.WEAPON_FLAME,
    //    WeaponType.WEAPON_WIPE,
    //    WeaponType.WEAPON_SHOVEL,
    //    WeaponType.WEAPON_DRONE,
    //    WeaponType.WEAPON_LASER,
    //    WeaponType.WEAPON_ENERGYSHILD
    //};

    private static readonly WeaponType[] _allWeapons =
    {
        //WeaponType.WEAPON_AEGIS,
        //WeaponType.WEAPON_BOMB,
        WeaponType.WEAPON_LASER,
        //WeaponType.WEAPON_ENERGYSHILD
    };

    private void Start()
    {

    }

    public void Init()
    {
        _playerHolderObj = AllManager.instance.GamemodeManager.Player.GetComponent<PlayerUnit>();
        _weaponFactoryObj = WeaponFactory.instance;

        _playerHolder = (IPlayerHolder)_playerHolderObj;
        _weaponFactory = (IWeaponFactory)_weaponFactoryObj;

        // 버튼에 인덱스 콜백 등록
        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            int idx = i; // 로컬 복사
            _choiceButtons[idx].Init(() => SelectedIndex(idx));
        }

        _itemPanel.SetActive(false);
    }

    /// <summary>레벨업 시 외부에서 호출</summary>
    public void OpenSelection()
    {
        // 1) 후보 풀 작성 (보유 무기는 제외)
        var candidates = new List<WeaponType>(_allWeapons);
        var ownedWeapons = _playerHolder.GetOwnedWeaponTypes();
        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            var weapon = ownedWeapons[i];
            if (weapon.CurrentLevel >= weapon.MaxLevel)
            {
                for (int j = candidates.Count - 1; j >= 0; j--)
                {
                    if (candidates[j] == weapon.WeaponType) candidates.RemoveAt(j);
                }
            }
        }


        // 2) 셔플 & 앞 3개 추출
        ShuffleUtils.Shuffle(candidates);
        int take = candidates.Count < 3 ? candidates.Count : 3;
        _currentChoices = new WeaponType[take];
        for (int i = 0; i < take; i++) _currentChoices[i] = candidates[i];

        // 3) 버튼 라벨·아이콘 세팅
        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            if (i < take)
            {
                _choiceButtons[i].gameObject.SetActive(true);
                var type = _currentChoices[i];
                var data = _weaponFactory.GetWeaponData(type);
                var name = data != null ? data.WeaponName : type.ToString();

                // 현재 레벨과 다음 레벨 스탯 비교 문자열 생성
                List<WeaponStatDiff> diffStats = null;
                if (data != null)
                {
                    var owned = ownedWeapons.FirstOrDefault(w => w.WeaponType == type);
                    int currentLevel = owned != null ? owned.CurrentLevel : 0;

                    WeaponAttribute nextAttr = data.LevelAttributes[Mathf.Clamp(currentLevel, 0, data.LevelAttributes.Count - 1)];
                    WeaponAttribute currAttr = currentLevel > 0 ? data.LevelAttributes[Mathf.Clamp(currentLevel - 1, 0, data.LevelAttributes.Count - 1)] : new WeaponAttribute();
                    diffStats = BuildDiffList(nextAttr, currAttr);
                }

                _choiceButtons[i].SetData(type, name, diffStats);
            }
            else
            {
                _choiceButtons[i].gameObject.SetActive(false);
            }
        }

        _itemPanel.SetActive(true);
    }

    /// <summary>무기 버튼이 눌릴 때 ISelectedIndex 인터페이스로 호출</summary>
    public void SelectedIndex(int index)
    {
        if (index < 0 || index >= _currentChoices.Length) return;

        WeaponType chosen = _currentChoices[index];

        // 1) 무기 생성 & 장착
        if (_playerHolderObj != null) _playerHolder.AddWeapon(chosen);

        // 2) 패널 숨김
        //_itemPanel.SetActive(false);
        AllManager.instance.UIManager.OnWeaponChosen();
    }

    private List<WeaponStatDiff> BuildDiffList(WeaponAttribute next, WeaponAttribute current)
    {
        var list = new List<WeaponStatDiff>();

        void AddFloat(string label, float diff)
        {
            if (Mathf.Approximately(diff, 0f)) return;
            list.Add(new WeaponStatDiff(label, diff));
        }

        void AddInt(string label, int diff)
        {
            if (diff == 0) return;
            list.Add(new WeaponStatDiff(label, diff));
        }

        AddFloat("Damage", next.FlatDamage - current.FlatDamage);
        AddFloat("Dmg%", next.DamageMultiplier - current.DamageMultiplier);
        AddFloat("Crit", next.CriticalDamage - current.CriticalDamage);
        AddFloat("Crit%", next.CriticalChance - current.CriticalChance);
        AddFloat("AS", next.AttackSpeedMultiplier - current.AttackSpeedMultiplier);
        AddInt("Count", next.ProjectileCountBonus - current.ProjectileCountBonus);
        AddFloat("Range", next.RangeBonus - current.RangeBonus);
        AddFloat("CD", next.AttackCooldown - current.AttackCooldown);
        AddFloat("Knock", next.KnockbackForce - current.KnockbackForce);
        AddInt("Pirece", next.WeaponDisableCount - current.WeaponDisableCount);
        AddFloat("Time", next.WeaponDisableTime - current.WeaponDisableTime);

        return list;
    }
}
