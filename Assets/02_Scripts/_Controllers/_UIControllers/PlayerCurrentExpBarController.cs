using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentExpBarController : MonoBehaviour
{
    [TabGroup("Image")][Tooltip("플레이어 경험치 바 컨트롤러")][SerializeField] private Image _playerCurrentExpBar;
    public Image PlayerCurrentExpBar { get { return _playerCurrentExpBar; } }

    [TabGroup("Controller")][Tooltip("플레이어 경험치 바 점멸 스피드")][SerializeField] private float _speed;     // 1.0f 기준 
    [TabGroup("Controller")][Tooltip("플레이어 경험치 바 점멸 스피드")][SerializeField] private float _elapsed;   // 누적 시간

    private Color _expBaseColor = new Color(0, 113, 246, 255);

    public void UpdatePlayerCurrentExpBar(float fillAmount)
    {
        _playerCurrentExpBar.fillAmount = fillAmount;
    }

    public void RevertPlayerCurreontExpBar()
    {
        _playerCurrentExpBar.fillAmount = 0f;
        _playerCurrentExpBar.color = _expBaseColor;
    }

    public void UpdateExpBarBlinking()
    {
        _playerCurrentExpBar.fillAmount = 1f;

        // 1) 시간 누적
        _elapsed += Time.deltaTime * _speed;

        // 2) 0↔1 사이를 부드럽게 왕복하는 PingPong 함수
        float blue = Mathf.PingPong(_elapsed, 1f);

        // 3) 컬러에 적용
        var c = _playerCurrentExpBar.color;
        c.b = blue;
        _playerCurrentExpBar.color = c;
    }
}
