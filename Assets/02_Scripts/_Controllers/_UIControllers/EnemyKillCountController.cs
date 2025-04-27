using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class EnemyKillCountController : MonoBehaviour
{
    [TabGroup("Image")][Tooltip("플레이어 경험치 바 컨트롤러")][SerializeField] private TextMeshProUGUI _enemyKillCountText;
    public TextMeshProUGUI EnemyKillCountText { get { return _enemyKillCountText; } }

    public void UpdateKillCounter(int killCount)
    {
        _enemyKillCountText.text = "\t : " + killCount;
    }
}
