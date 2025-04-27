using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class EnemyKillCountController : MonoBehaviour
{
    [TabGroup("Image")][Tooltip("플레이어 경험치 바 컨트롤러")][SerializeField] private TextMeshProUGUI _enemyKillCountText;
    public TextMeshProUGUI EnemyKillCountText { get { return _enemyKillCountText; } }

    public void UpdateKillCounter()
    {
        int currentCount;

        // 숫자만 따로 저장할 수 있도록 정규식이나 숫자만 추출하는 방법을 쓸 수도 있어
        string digits = System.Text.RegularExpressions.Regex.Replace(_enemyKillCountText.text, @"[^\d]", "");

        if (!int.TryParse(digits, out currentCount))
        {
            currentCount = 0;
        }

        currentCount++;

        _enemyKillCountText.text = "\t : " + currentCount;
    }
}
