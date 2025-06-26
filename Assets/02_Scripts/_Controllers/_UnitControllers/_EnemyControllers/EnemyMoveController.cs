using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 유닛 이동을 담당하는 컨트롤러
/// A* 알고리즘을 이용해 플레이어를 추적한다
/// </summary>
public class EnemyMoveController : MonoBehaviour, IMoveable, IRotationable
{
    [TabGroup("Target")][SerializeField] private GameObject _player;               // 추적 대상
    public GameObject Player => _player;

    [TabGroup("Target")][SerializeField] private IPositionProvider _playerPosition; // 대상 위치 인터페이스
    public IPositionProvider PlayerPosition => _playerPosition;

    [TabGroup("Target")][SerializeField] private float _angle;                     // 회전 각도
    public float Angle => _angle;

    [TabGroup("EnemyMoveDirection")][SerializeField] private Vector3 _moveDirection; // 이동 방향
    public Vector3 MoveDirection { get => _moveDirection; set => _moveDirection = value; }

    [TabGroup("EnemyMoveDirection")][SerializeField] private Vector3 _rotatoinDirection; // 회전 방향
    public Vector3 RotationDirection { get => _rotatoinDirection; set => _rotatoinDirection = value; }

    [TabGroup("Interface")][SerializeField] private IUnitAttribute<UnitAttribute> _enemyAttribute; // 적 능력치
    public IUnitAttribute<UnitAttribute> EnemyAttribute => _enemyAttribute;

    [TabGroup("PathFinding")][SerializeField] private MonoBehaviour _mapSource;    // 맵 제공자 객체

    private IGridMap _map;                                // 맵 인터페이스
    private IPathFinder _pathFinder;                      // 경로 탐색기
    private List<Vector2> _path = new List<Vector2>();    // 계산된 경로
    private int _pathIndex;                               // 현재 따라갈 노드 인덱스
    private float _repathTimer;                           // 경로 갱신 타이머
    [SerializeField] private float _repathInterval = 0.5f; // 경로 다시 계산 주기

    private void Start()
    {
        _player = AllManager.instance.GamemodeManager.Player;
        if (_mapSource != null)
        {
            _map = _mapSource as IGridMap;
            if (_map != null)
            {
                _pathFinder = new AStarPathFinder(_map);
            }
        }
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        if (CheckEnemyMoveTarget() == false) return;

        _repathTimer += Time.deltaTime;
        if (_repathTimer >= _repathInterval)
        {
            _repathTimer = 0f;
            if (CheckTargetPosition() && _pathFinder != null)
            {
                _path = _pathFinder.FindPath(transform.position, _playerPosition.Position);
                _pathIndex = 0;
            }
        }

        Move();
        Rotate();
    }

    private bool CheckEnemyMoveTarget()
    {
        if (_player == null)
        {
            _player = AllManager.instance.GamemodeManager.Player;
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
            Debug.LogError("IEnemyAttribute 인터페이스를 찾을 수 없습니다!");
            return false;
        }
    }

    private bool CheckTargetPosition()
    {
        if (_player != null && _player.TryGetComponent<IPositionProvider>(out var provider))
        {
            _playerPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 인터페이스를 찾을 수 없습니다!");
            return false;
        }
    }

    /// <summary>
    /// 실제 이동 처리
    /// </summary>
    public void Move()
    {
        if (CheckEnemyAttribute() == false) return;
        if (_path == null || _pathIndex >= _path.Count) return;

        Vector3 target = _path[_pathIndex];
        _moveDirection = (target - transform.position).normalized;
        transform.position += _moveDirection * (_enemyAttribute.Attributes.UnitMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            _pathIndex++;
        }
    }

    /// <summary>
    /// 회전 처리
    /// </summary>
    public void Rotate()
    {
        _rotatoinDirection = _moveDirection;
        _angle = Mathf.Atan2(_rotatoinDirection.y, _rotatoinDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(_angle - 90, Vector3.forward);
    }
}
