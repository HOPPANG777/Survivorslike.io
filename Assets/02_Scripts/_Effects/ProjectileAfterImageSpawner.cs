using UnityEngine;

/// <summary>
/// 투사체의 이동 경로에 잔상을 생성하는 컴포넌트
/// </summary>
[RequireComponent(typeof(IAfterImageTarget))]
public class ProjectileAfterImageSpawner : MonoBehaviour
{
    /// <summary>대상 인터페이스</summary>
    private IAfterImageTarget _target;
    /// <summary>잔상 타이머</summary>
    private float _timer;

    private void Awake()
    {
        _target = this.GetComponent<IAfterImageTarget>();
    }

    private void Update()
    {
        if (_target == null) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _timer = _target.AfterImageInterval;
            Spawn();
        }
    }

    /// <summary>잔상 생성 처리</summary>
    private void Spawn()
    {
        var renderer = _target.Renderer;
        if (renderer == null) return;
        var effect = AllManager.instance.ParticleFactory.Create(ParticleType.PARTICLE_DASH, transform.position, Quaternion.identity);
        var after = effect.GetComponent<DashAfterImage>();
        after.Initialize(renderer.sprite, Color.white, renderer.sortingOrder - 1, _target.AfterImageDuration, renderer.size);
    }
}
