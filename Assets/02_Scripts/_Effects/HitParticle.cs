using Sirenix.OdinInspector;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
    [TabGroup("HitParticle")][Tooltip("지속시간")][SerializeField] private float _lifeTime = 2.0f;
    [TabGroup("HitParticle")][Tooltip("현재지속시간")][SerializeField] private float _duration = 0.0f;

    void LateUpdate()
    {
        _duration += Time.deltaTime;
        if (_duration >= _lifeTime)
        {
            AllManager.instance.ParticleFactory.Release(this.gameObject);
        }
        else
        {
            _duration = 0.0f;
        }
    }
}
