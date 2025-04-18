using UnityEngine;

//==============================================================================
// PlayerMoveController
//==============================================================================
// Player의 이동을 담당하는 Controller
//
//==============================================================================
public class PlayerMoveController : MonoBehaviour, IPositionProvider, IMoveable
{
    [Header("Target")]
    [SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } }

    [SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    [Header("Interface")]
    [SerializeField] private Vector3 _position => this.transform.position;
    public Vector3 Position { get { return _position; } }

    [SerializeField] private IPlayerAttribute _playerAttribute;
    public IPlayerAttribute PlayerAttribute { get { return _playerAttribute; } }

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
        if (_player.TryGetComponent<IPlayerAttribute>(out var attribute))
        {
            _playerAttribute = attribute;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (CheckPlayerMove() == false) return;
        if (CheckPlayerAttribute() == false) return;

        // 입력
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");

        // 방향 벡터
        Vector3 nextVector = _moveDirection.normalized * _playerAttribute.Attributes.Base.UnitMoveSpeed * Time.deltaTime;

        // 이동
        this.transform.position = this.transform.position + nextVector;
    }    
}
