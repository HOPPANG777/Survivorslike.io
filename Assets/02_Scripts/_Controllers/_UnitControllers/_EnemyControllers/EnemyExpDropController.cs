using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyExpDropController : MonoBehaviour
{
    [TabGroup("Enemy")][Tooltip("적")][SerializeField] private GameObject _enemy;
    public GameObject Enemy { get { return _enemy; } set { _enemy = value; } }

    [TabGroup("Enemy")][Tooltip("적의 현재 포지션")][SerializeField] private IPositionProvider _enemyPosition;
    public IPositionProvider EnemyPosition { get { return _enemyPosition; } set { _enemyPosition = value; } }

    [TabGroup("EnemyExp")][SerializeField] private float _expValue;
    public float ExpValue { get { return _expValue; } set { _expValue = value; } }

    [TabGroup("EnemyExp")][SerializeField] private GameObject _enemyGem;
    public GameObject EnemyGem { get { return _enemyGem; } set { _enemyGem = value; } }

    [TabGroup("Interface")][SerializeField] private IUnitAttribute<EnemyableAttribute> _enemyAttribute;
    public IUnitAttribute<EnemyableAttribute> EnemyAttribute { get { return _enemyAttribute; } set { _enemyAttribute = value; } }  

    public void Start()
    {
        _enemy = this.gameObject;
        if (CheckEnemy() == false) return;
        if (CheckEnemyAttribute() == false) return;
    }

    public void Update()
    {
        if (CheckEnemyPosition() == false) return;


    }

    private bool CheckEnemy()
    {
        if (!_enemy)
        {
            //_enemy = this.gameObject;
            Debug.LogError("Enemy 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }    

    private bool CheckEnemyPosition()
    {
        if (this.TryGetComponent<IPositionProvider>(out var provider))
        {
            _enemyPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    private bool CheckEnemyAttribute()
    {
        if (this.TryGetComponent<IUnitAttribute<EnemyableAttribute>>(out var attribute))
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

    


}
