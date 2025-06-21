using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ParticleFactory : SerializedSingleton<ParticleFactory>, IParticleFactory
{
    [TabGroup("Particle Factory")]
    [SerializeField]
    private List<ParticlePrefabEntry> _entries = new List<ParticlePrefabEntry>();

    private IPoolManager _pool;
    private Dictionary<ParticleType, GameObject> _map;
    private readonly List<GameObject> _activeParticles = new List<GameObject>();
    private bool _initialized;

    private void Start()
    {
        //Init();
    }

    public void Init()
    {
        if (_initialized) return;

        _pool = PoolManager.instance;
        _map = new Dictionary<ParticleType, GameObject>(_entries.Count);
        for (int i = 0; i < _entries.Count; i++)
        {
            var e = _entries[i];
            if (e.Prefab != null)
            {
                _map[e.Type] = e.Prefab;
            }
        }

        _initialized = true;
    }

    public GameObject Create(ParticleType type, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!_initialized)
        {
            Init();
        }
        if (!_map.TryGetValue(type, out var prefab)) return null;
        var go = _pool.Get(prefab);
        if (parent != null) go.transform.SetParent(parent, false);
        go.transform.position = position;
        go.transform.rotation = rotation;
        _activeParticles.Add(go);
        return go;
    }

    public void Release(GameObject particle)
    {
        if (particle == null) return;
        if (_activeParticles.Contains(particle)) _activeParticles.Remove(particle);
        particle.SetActive(false);
        _pool.Return(particle);
    }

    public void ReleaseAll()
    {
        for (int i = _activeParticles.Count - 1; i >= 0; i--)
        {
            var obj = _activeParticles[i];
            if (obj != null)
            {
                obj.SetActive(false);
                _pool.Return(obj);
            }
        }
        _activeParticles.Clear();
    }
}
