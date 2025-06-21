using Sirenix.OdinInspector;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
    [TabGroup("HitParticle")][Tooltip("파티클이 유지될 시간")][SerializeField] private float _lifeTime = 2.0f;
    [TabGroup("HitParticle")][Tooltip("현재 경과 시간")][SerializeField] private float _duration = 0.0f;

    // 매 프레임 경과 시간을 누적하고 수명이 다하면 풀로 반환한다
    private void LateUpdate()
    {
        _duration += Time.deltaTime;
        if (_duration >= _lifeTime)
        {
            _duration = 0.0f; // 다음 재사용을 위해 시간 초기화
            AllManager.instance.ParticleFactory.Release(this.gameObject);
        }
    }
}
