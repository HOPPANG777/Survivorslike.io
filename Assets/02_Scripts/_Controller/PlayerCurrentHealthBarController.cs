using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentHealthBarController : MonoBehaviour
{
    [TabGroup("Image")][Tooltip("플레이어 체력바 컨트롤러")][SerializeField] private Image _playerCurrentHealthBar;
    public Image PlayerCurrentHealthBar { get { return _playerCurrentHealthBar; } }

    public void UpdatePlayerHealthBarImage(float fillAmount)
    {
        _playerCurrentHealthBar.fillAmount = fillAmount;
    }
}
