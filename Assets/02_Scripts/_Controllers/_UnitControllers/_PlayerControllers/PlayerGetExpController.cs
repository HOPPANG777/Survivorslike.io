using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public class PlayerGetExpController : MonoBehaviour
{
    [TabGroup("Player")][SerializeField] private PlayerUnit _player;
    public PlayerUnit Player { get { return _player; } }

    [TabGroup("EnemyExp")][SerializeField] private ItemBase _enemyExp;
    public ItemBase EnemyExp { get { return _enemyExp; } set { _enemyExp = value; } }

    private void Start()
    {
        _player = AllManager.instance.GamemodeManager.Player.GetComponent<PlayerUnit>();
    }

    private void Update()
    {
        if (PlayerCheck() == false) return;
    }

    private bool PlayerCheck()
    {
        if (!_player)
        {
            Debug.LogError("Player가 할당되지 않았습니다!");
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddExperience(int expAmount)
    {
        
    }

}
