using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyExpDropController : MonoBehaviour
{
    [TabGroup("Enemy")][Tooltip("적")][SerializeField] private GameObject _enemy;
    [TabGroup("Enemy")][Tooltip("적의 현재 포지션")][SerializeField] private IPositionProvider _enemyPosition;
    [TabGroup("Exp")][SerializeField] private ItemBaseSO _expData;
   
    private bool CheckEnemy()
    {
        if (!_enemy)
        {
            //_enemy = this.gameObject;
            //Debug.LogError("Enemy 컴포넌트를 찾을 수 없습니다!");
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

    public void EnemyDropExp()
    {
        if (CheckEnemyPosition() == false) return;
        //if (CheckEnemy() == false) return;

        if (_expData == null || _enemyPosition == null)
        {
            Debug.LogError("EnemyDropExp: Item Data 또는 SpawnPoint 할당 누락");
            return;
        }

        // 팩토리에서 꺼내 쓰기
        var go = ItemFactory.instance.Create(_expData, _enemyPosition.Position);

        if (go == null)
        {
            Debug.LogError("EnemyDropExp: ItemFactory에서 생성 실패");
            return;
        }
    }
    
}
