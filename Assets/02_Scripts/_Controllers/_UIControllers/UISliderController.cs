using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

public class UISliderController : MonoBehaviour, IUISlider, IMoveable, ISelectedIndex
{
    [TabGroup("TitleUIPanel")][Tooltip("이동할 거리(px)(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _slideDistance; // 200이 기본
    public float SlideDistance { get { return _slideDistance / 100.0f; } set { _slideDistance = (int)value; } }

    [TabGroup("TitleUIPanel")][Tooltip("애니메이션 소요 시간(s)(그리고 100 분율로 보십시오 따라서 50일댸 0.5f가 됩니다.)")][SerializeField] private int _moveDuration; // 0.5가 기본
    public float MoveDuration { get { return _moveDuration / 100.0f; } set { _moveDuration = (int)value; } }

    [TabGroup("Direction")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    [TabGroup("Direction")][Tooltip("슬라이드 방향 선택")][SerializeField] private SlideDirection _slideDirection = SlideDirection.SLIDE_UP;

    //
    [TabGroup("UI")][SerializeField] private RectTransform _rectTransform;
    [TabGroup("UI")][SerializeField] private Vector3 _startPos;
    [TabGroup("UI")][SerializeField] private Vector3 _targetPos;
    [TabGroup("UI")][SerializeField] private float _elapsedTime;
    [TabGroup("UI")][SerializeField] private bool _isSliding;
    //

    void Update()
    {
        if (!_isSliding) return;

        _elapsedTime += Time.deltaTime;
        float t = _elapsedTime / MoveDuration;
        if (t > 1f) t = 1f;

        _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _targetPos, t);

        if (t >= 1f)
        {
            _isSliding = false;
        }
    }

    public void SelectedIndex(int index)
    {
        switch (index)
        {
            case 0: _slideDirection = SlideDirection.SLIDE_UP; break;
            case 1: _slideDirection = SlideDirection.SLIDE_DOWN; break;
            case 2: _slideDirection = SlideDirection.SLIDE_LEFT; break;
            case 3: _slideDirection = SlideDirection.SLIDE_RIGHT; break;
        }
    }

    public void Move()
    {
        Slide(_slideDirection);
    }

    public void Slide(SlideDirection direction)
    {
        switch (direction)
        {
            case SlideDirection.SLIDE_UP: _moveDirection = Vector2.up; break;
            case SlideDirection.SLIDE_DOWN: _moveDirection = Vector2.down; break;
            case SlideDirection.SLIDE_LEFT: _moveDirection = Vector2.left; break;
            case SlideDirection.SLIDE_RIGHT: _moveDirection = Vector2.right; break;
            default: _moveDirection = Vector2.zero; break;
        }
        StartSlide(_moveDirection);
    }

    private void StartSlide(Vector3 dirVec)
    {
        if (_isSliding) return; // 이미 동작 중이면 무시

        _startPos = _rectTransform.anchoredPosition;
        _targetPos = _startPos + dirVec * SlideDistance;
        _elapsedTime = 0f;
        _isSliding = true;
    }

    public bool IsSliding => _isSliding;
}
