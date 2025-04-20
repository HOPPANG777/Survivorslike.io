using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentExpBarController : MonoBehaviour
{
    [TabGroup("Image")][Tooltip("플레이어 경험치 바 컨트롤러")][SerializeField] private Image _playerCurrentExpBar;
    public Image PlayerCurrentExpBar { get { return _playerCurrentExpBar; } }

    [TabGroup("Controller")][Tooltip("플레이어 경험치 바 점멸 스피드")][SerializeField] private float _speed;     // 1.0f 기준 
    [TabGroup("Controller")][Tooltip("플레이어 경험치 바 점멸 스피드")][SerializeField] private float _elapsed;   // 누적 시간

    public void UpdatePlayerCurrentExpBar(float fillAmount)
    {
        _playerCurrentExpBar.fillAmount = fillAmount;
    }

    //public void UpdateExpBarBlinking()
    //{
    //    Color color = _playerCurrentExpBar.color;

    //    if (_blinking)
    //    {
    //        float Max_B = 1.0f;

    //        _currentBlue += _speed * Time.deltaTime;
    //        if (_currentBlue >= Max_B)
    //        {
    //            _currentBlue = Max_B;
    //            _blinking = false;
    //        }
    //    }
    //    else
    //    {
    //        float Min_B = 0.0f;

    //        _currentBlue -= _speed * Time.deltaTime;
    //        if (_currentBlue <= Min_B)
    //        {
    //            _currentBlue = Min_B;
    //            _blinking = true;
    //        }
    //    }

    //    color.b = _currentBlue;
    //    _playerCurrentExpBar.color = color;
    //}

    public void UpdateExpBarBlinking()
    {
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
