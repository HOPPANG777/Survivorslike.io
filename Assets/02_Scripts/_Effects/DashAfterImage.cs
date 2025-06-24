using UnityEngine;

public class DashAfterImage : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private float _duration;
    private float _timer;

    public void Initialize(Sprite sprite, Color color, int sortingOrder, float duration, Vector2 size)
    {
        _renderer = gameObject.AddComponent<SpriteRenderer>();
        _renderer.sprite = sprite;
        _renderer.drawMode = SpriteDrawMode.Sliced;
        _renderer.size = size; // Adjust size as needed
        _renderer.sortingOrder = sortingOrder;
        _renderer.color = color;
        //var brightColor = new Color(Mathf.Min(color.r * 1.5f, 1f), Mathf.Min(color.g * 1.5f, 1f), Mathf.Min(color.b * 1.5f, 1f), 1f);
        //_renderer.color = brightColor;
        _duration = duration;
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_renderer != null)
        {
            Color c = _renderer.color;
            c.a = Mathf.Lerp(1f, 0f, _timer / _duration);
            _renderer.color = c;
        }
        if (_timer >= _duration)
        {
            AllManager.instance.ParticleFactory.Release(this.gameObject);
        }
    }
}
