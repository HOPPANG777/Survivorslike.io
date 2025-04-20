using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerCurrentLevelCountController : MonoBehaviour
{
    [TabGroup("Image")][Tooltip("플레이어 경험치 바 컨트롤러")][SerializeField] private TextMeshProUGUI _playerCurrentLevelCountText;
    public TextMeshProUGUI PlayerCurrentLevelCountText { get { return _playerCurrentLevelCountText; } }

    public void UpdateKillCounter(int count)
    {
        _playerCurrentLevelCountText.text = "Lv. " + count;
    }
}
