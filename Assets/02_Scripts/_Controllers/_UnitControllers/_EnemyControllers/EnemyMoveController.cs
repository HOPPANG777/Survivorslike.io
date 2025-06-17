using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour, IMoveable, IRotationable
{
    [TabGroup("Target")][SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } }

    [TabGroup("Target")][Tooltip("타겟 포지션")][SerializeField] private IPositionProvider _playerPosition;
    public IPositionProvider PlayerPosition { get { return _playerPosition; } }

    [TabGroup("Target")][Tooltip("타겟의 방향")][SerializeField] private float _angle;
    public float Angle { get { return _angle; } }

    [TabGroup("EnemyMoveDirection")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    [TabGroup("EnemyMoveDirection")][SerializeField] private Vector3 _rotatoinDirection;
    public Vector3 RotationDirection { get { return _rotatoinDirection; } set { _rotatoinDirection = value; } }

    [TabGroup("Interface")][SerializeField] private IUnitAttribute<UnitAttribute> _enemyAttribute;
    public IUnitAttribute<UnitAttribute> EnemyAttribute { get { return _enemyAttribute; } }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        if (CheckEnemyMoveTarget() == false) return;
        Move();
        Rotate();
    }

    private bool CheckEnemyMoveTarget()
    {
        if (_player == null)
        {
            _player = AllManager.instance.GamemodeManager.Player;
            //Debug.LogError("Player 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckEnemyAttribute()
    {
        if (this.TryGetComponent<IUnitAttribute<UnitAttribute>>(out var attribute))
        {
            _enemyAttribute = attribute;
            return true;
        }
        else
        {
            Debug.LogError("IEnemyAttribute 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    private bool CheckTargetPosition()
    {
        if (_player.TryGetComponent<IPositionProvider>(out var provider))
        {
            _playerPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    public void Move()
    {
        if (CheckEnemyAttribute() == false) return;
        if (CheckTargetPosition() == false) return;
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        // 방향
        _moveDirection = (_playerPosition.Position - this.transform.position).normalized;

        // 이동
        this.transform.position = this.transform.position + (_moveDirection * (Random.Range(_enemyAttribute.Attributes.UnitMoveSpeed, _enemyAttribute.Attributes.UnitMoveSpeed) * Time.deltaTime));
    }

    public void Rotate()
    {
        _rotatoinDirection = _moveDirection;

        // 각도 계산
        _angle = Mathf.Atan2(_rotatoinDirection.y, _rotatoinDirection.x) * Mathf.Rad2Deg;

        // 회전
        this.transform.rotation = Quaternion.AngleAxis(_angle - 90, Vector3.forward);
    }

}
