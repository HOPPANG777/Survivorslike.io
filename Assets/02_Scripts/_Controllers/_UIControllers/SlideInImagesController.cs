using System.Collections.Generic;
using UnityEngine;

public class SlideInImagesController : MonoBehaviour, IMoveable, ISetStatus
{
    [Tooltip("바 위에 놓인 3개의 이미지들")]
    [SerializeField] private List<RectTransform> _images;

    [Tooltip("하얀 바(RectTransform)")]
    [SerializeField] private RectTransform _barRect;

    [Tooltip("한 이미지가 슬라이드되는 시간")]
    [SerializeField] private float _moveDuration = 1.2f;

    [Tooltip("이미지 사이 슬라이드 딜레이")]
    [SerializeField] private float _delayBetween = 0.1f;

    [Tooltip("Ease 곡선")]
    [SerializeField] private AnimationCurve _ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public Vector3 MoveDirection { get; set; }

    // 내부 상태
    private Vector2[] _initialPositions;  // 바 위 원래 위치
    private Vector2[] _startPositions;    // 슬라이드 시작 위치 (바 밖)
    private Vector2[] _endPositions;      // 슬라이드 도착 위치 (바 위)
    private int _currentIndex;
    private float _timer;

    private enum Phase { Move, Delay, Done }
    private Phase _phase;

    private void Start()
    {
        int n = _images.Count;
        _initialPositions = new Vector2[n];
        _startPositions = new Vector2[n];
        _endPositions = new Vector2[n];

        // 1) Awake 시점에 “바 위 원래 위치” 저장
        for (int i = 0; i < n; i++) _initialPositions[i] = _images[i].anchoredPosition;

        // 2) 도착 위치(endPositions)는 원래 위치와 같게 설정
        if (n > 0) _endPositions[0] = new Vector2(640f, _initialPositions[0].y);
        if (n > 1) _endPositions[1] = new Vector2(0f, _initialPositions[1].y);
        if (n > 2) _endPositions[2] = new Vector2(-640f, _initialPositions[2].y);
    }

    public void SetStatus()
    {
        // 배열만 미리 할당
        for (int i = 0, n = _images.Count; i < n; i++) _images[i].anchoredPosition = _initialPositions[i];
        enabled = false;    // Update 멈춤
        _phase = Phase.Done;
    }

    /// <summary>
    /// 초기화 → 애니메이션 재생
    /// </summary>
    public void Move()
    {
        int n = _images.Count;
        float barW = _barRect.rect.width;

        // 목표 위치 계산 (바 안에서 균등 배치)
        float spacing = barW / (n + 1);
        for (int i = 0; i < n; i++)
        {
            float x = -barW * 0.5f + spacing * (i + 1);
            _endPositions[i] = new Vector2(x, 0f);

            // 시작 위치: 왼쪽 바깥
            float offX = -barW * 0.5f - 100f;
            _startPositions[i] = new Vector2(offX, 0f);

            // 이미지 위치 리셋
            _images[i].anchoredPosition = _startPositions[i];
        }

        // 상태 초기화
        _currentIndex = 0;
        _timer = 0f;
        _phase = Phase.Move;

        // Enable 해서 Update 동작하도록
        enabled = true;
    }

    private void Update()
    {
        if (_phase == Phase.Done) return;
        if (_currentIndex >= _images.Count)
        {
            _phase = Phase.Done;
            enabled = false;
            return;
        }

        _timer += Time.deltaTime;

        switch (_phase)
        {
            case Phase.Move:
                if (_timer < _moveDuration)
                {
                    float t = _timer / _moveDuration;
                    float e = _ease.Evaluate(t);
                    var img = _images[_currentIndex];
                    img.anchoredPosition = Vector2.LerpUnclamped(_startPositions[_currentIndex], _endPositions[_currentIndex], e
                    );
                }
                else
                {
                    _images[_currentIndex].anchoredPosition = _endPositions[_currentIndex];
                    _timer = 0f;
                    _phase = Phase.Delay;
                }
                break;

            case Phase.Delay:
                if (_timer >= _delayBetween)
                {
                    _timer = 0f;
                    _currentIndex++;
                    _phase = Phase.Move;
                }
                break;
        }
    }
}