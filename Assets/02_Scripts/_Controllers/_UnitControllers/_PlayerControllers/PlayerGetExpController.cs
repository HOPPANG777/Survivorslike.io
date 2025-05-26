using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerUnit))]
public class PlayerGetExpController : MonoBehaviour
{
    [TabGroup("Player")][SerializeField] private PlayerUnit _player;
    public PlayerUnit Player => _player;

    private PlayerLevelUpController _levelController;

    private void Awake()
    {
        // 씬에 하나만 있는 AllManager 에서 플레이어를 가져오거나,
        // 미리 인스펙터에 드래그해 두셔도 됩니다.
        _player = AllManager.instance.GamemodeManager.Player.GetComponent<PlayerUnit>();

        // 레벨업 담당 컴포넌트 캐싱
        _levelController = GetComponent<PlayerLevelUpController>();
    }

    private bool PlayerCheck()
    {
        if (_player == null)
        {
            Debug.LogError("Player가 할당되지 않았습니다!");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 외부(ExpGemSO.Apply 등)에서 호출합니다.
    /// </summary>
    public void AddExperience(int expAmount)
    {
        if (!PlayerCheck()) return;

        // 1) struct 복사
        var attr = _player.Attributes;

        // 2) 현재 경험치 증가
        attr.PlayerCurrentExp += expAmount;

        // 3) 다시 할당
        _player.Attributes = attr;

        // 4) 레벨업 시도 (남은 경험치, UI 업데이트 포함)
        if (_levelController != null)
            _levelController.TryLevelUp();
        else
            Debug.LogWarning("PlayerLevelController가 없습니다. 레벨업이 동작하지 않습니다.");
    }
}
