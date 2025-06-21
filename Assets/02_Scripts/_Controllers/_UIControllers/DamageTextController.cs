using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    [TabGroup("Damage Text")][SerializeField] private TextMeshProUGUI _text;
    [TabGroup("Damage Text")][SerializeField] private Vector3 _offset = Vector3.up;
    [TabGroup("Damage Text")][SerializeField] private float _floatHeight = 1f;
    [TabGroup("Damage Text")][SerializeField] private float _duration = 1f;
    [TabGroup("Damage Text")][SerializeField] private AnimationCurve _moveCurve = AnimationCurve.EaseInOut(0,0,1,1);

    private Camera _uiCamera;
    private Vector3 _startWorldPos;
    private float _startTime;
    private bool _playing;

    public void Play(float value, Vector3 worldPosition, Color color)
    {
        if (_text != null)
        {
            _text.text = Mathf.RoundToInt(value).ToString();
            _text.color = color;
        }
        if (_uiCamera == null) _uiCamera = Camera.main;
        _startWorldPos = worldPosition + _offset;
        _startTime = Time.time;
        _playing = true;
        UpdatePosition(_startWorldPos);
    }

    private void Update()
    {
        if (!_playing) return;
        float t = (Time.time - _startTime) / _duration;
        if (t >= 1f)
        {
            _playing = false;
            AllManager.instance.UIFactory.Release(this.gameObject);
            return;
        }
        float y = _moveCurve.Evaluate(t) * _floatHeight;
        Vector3 pos = _startWorldPos + Vector3.up * y;
        UpdatePosition(pos);
        if (_text != null)
        {
            var c = _text.color;
            c.a = 1f - t;
            _text.color = c;
        }
    }

    private void UpdatePosition(Vector3 worldPos)
    {
        if (_uiCamera != null && GetComponentInParent<Canvas>()?.renderMode != RenderMode.WorldSpace)
        {
            transform.position = _uiCamera.WorldToScreenPoint(worldPos);
        }
        else
        {
            transform.position = worldPos;
        }
    }
}
