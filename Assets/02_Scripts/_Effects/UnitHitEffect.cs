using UnityEngine;

//==============================================================================
// UnitHitEffect
//==============================================================================
// 유닛이 피격될 때 색상을 어둡게 변경하고 파티클을 생성해주는 스크립트
//==============================================================================
public class UnitHitEffect : MonoBehaviour
{
    [Tooltip("피격 시 어두워질 색상")]
    [SerializeField] private Color _hitColor = Color.gray;
    [Tooltip("원래 색상으로 돌아갈 시간")]
    [SerializeField] private float _duration = 0.1f;
    [Tooltip("스프라이트 렌더러")]
    [SerializeField] private SpriteRenderer _renderer;

    private Color _originColor;
    private float _recoverTime;
    private IParticleFactory _factory;

    private void Awake()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
        if (_renderer != null) _originColor = _renderer.color;
        _factory = ParticleFactory.instance;
    }

    // 피격 이펙트 재생
    public void PlayEffect(Vector3 position, Vector3 direction)
    {
        if (_renderer != null)
        {
            _renderer.color = _hitColor;
            _recoverTime = Time.time + _duration;
        }

        if (_factory != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _factory.Create(ParticleType.PARTICLE_HIT, position, Quaternion.Euler(0f, 0f, angle));
        }
    }

    private void Update()
    {
        if (_renderer != null && Time.time >= _recoverTime && _renderer.color != _originColor)
        {
            _renderer.color = _originColor;
        }
    }
}
