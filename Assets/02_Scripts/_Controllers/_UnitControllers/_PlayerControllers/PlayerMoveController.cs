using Sirenix.OdinInspector;
using UnityEngine;

//==============================================================================
// PlayerMoveController
//==============================================================================
// Player의 이동을 담당하는 Controller
//
//==============================================================================
public class PlayerMoveController : MonoBehaviour, IMoveable, ISetStatus
{
    [TabGroup("Target")][SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } }

    [TabGroup("Target")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    [TabGroup("Interface")][SerializeField] private IUnitAttribute<UnitAttribute> _playerAttribute;
    public IUnitAttribute<UnitAttribute> PlayerAttribute { get { return _playerAttribute; } }

    public void SetStatus()
    {
        _player = AllManager.instance.GamemodeManager.Player;
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Move();
    }

    private bool CheckPlayerMove()
    {
        if (_player == null)
        {
            Debug.LogError("Player가 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckPlayerAttribute()
    {
        if (_player.TryGetComponent<IUnitAttribute<UnitAttribute>>(out var attribute))
        {
            _playerAttribute = attribute;
            return true;
        }
        else
        {
            Debug.LogError("IPlayerAttribute 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    public void Move()
    {
        if (CheckPlayerMove() == false) return;
        if (CheckPlayerAttribute() == false) return;
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        // 입력
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");

        // 방향 벡터
        Vector3 nextVector = _moveDirection.normalized * _playerAttribute.Attributes.UnitMoveSpeed * Time.deltaTime;

        // 이동
        this.transform.position = this.transform.position + nextVector;
    }




}
