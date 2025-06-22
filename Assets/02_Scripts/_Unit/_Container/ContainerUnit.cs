using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//==============================================================================
// ContainerUnit
//==============================================================================
// UnitBase를 상속받은 단순 상자 유닛
// - 밀리지 않고 넉백도 받지 않음
// - 공격 기능이 없으며 3번 피격 시 파괴됨
// - 파괴될 때 설정된 아이템 중 무작위로 하나를 생성
//==============================================================================
public class ContainerUnit : UnitBase
{
    [Header("Container Unit")][TabGroup("Container Unit")][Tooltip("맞으면 감소하는 최대 내구도 값")][SerializeField] private int _maxHitCount = 3;
    [TabGroup("Container Unit")][Tooltip("현재 충돌한 횟수")][SerializeField] private int _currentHitCount;
    [TabGroup("Container Unit")][Tooltip("파괴 시 생성될 아이템 목록")][SerializeField] private List<ItemBaseSO> _dropItems = new List<ItemBaseSO>();

    // 공격하지 않는 유닛이므로 공격력은 0으로 고정
    public override float AttackPower { get { return 0f; } }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetStatus()
    {
        base.SetStatus();

        // 내구도는 충돌 횟수로 누적 관리하므로 0부터 시작
        _currentHitCount = 0;
        _unitAttribute.UnitHealth = _maxHitCount;
        _unitAttribute.UnitMaxHealth = _maxHitCount;
        _unitAttribute.UnitCurrentHealth = _maxHitCount;

        // 공격 능력과 넉백 수치를 0으로 고정
        _unitAttribute.UnitDamage = 0f;
        _unitAttribute.UnitKnockbackForce = 0f;

        // 피격 후 무적 시간 제거 (연속 타격 가능)
        _invincibleDuration = 0f;
    }

    public override void Hitted(float damageValue)
    {
        // 이미 파괴되었다면 처리하지 않음
        if (IsDead) return;

        // 피격 이펙트 처리
        var effect = this.GetComponent<UnitHitEffect>();
        if (effect != null)
        {
            effect.PlayEffect(this.transform.position, Vector3.zero);
        }

        // 데미지 텍스트 표시(고정값 1)
        var parent = AllManager.instance.UIPoolCanvas != null ? AllManager.instance.UIPoolCanvas.CanvasRoot : null;
        var go = AllManager.instance.UIFactory.Create(UIType.UI_DAMAGE_TEXT, parent);
        if (go != null)
        {
            var dmg = go.GetComponent<DamageTextController>();
            if (dmg != null)
            {
                dmg.Play(1f, this.transform.position, Color.white);
            }
        }

        // 충돌 횟수 누적
        _currentHitCount += 1;
        _unitAttribute.UnitCurrentHealth = _maxHitCount - _currentHitCount;

        // 누적된 충돌 횟수가 최대에 도달하면 파괴 처리
        if (_currentHitCount >= _maxHitCount)
        {
            Dead();
        }
    }

    public override void Dead()
    {
        if (IsDead) return;
        base.Dead();

        // 아이템 무작위 생성
        int count = _dropItems.Count;
        if (count > 0)
        {
            int index = Random.Range(0, count);
            ItemBaseSO item = _dropItems[index];
            if (item != null)
            {
                AllManager.instance.ItemFactory.Create(item, this.transform.position);
            }
        }

        // 풀 매니저로 반환
        AllManager.instance.UnitFactory.Release(this.gameObject.GetComponent<ContainerUnit>());
    }
}
