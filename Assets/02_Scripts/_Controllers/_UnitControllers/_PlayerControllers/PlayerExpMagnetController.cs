using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerExpMagnetController : MonoBehaviour, IExpMagnet, ISetStatus
{
    [SerializeField] private GameObject _player;
    private float _timer;
    private float _speed;

    public void SetStatus()
    {
        _player = AllManager.instance.GamemodeManager.Player;
        _timer = 0f;
        _speed = 0f;
    }

    public void StartMagnet(float duration, float speed)
    {
        _timer = duration;
        _speed = speed;
    }

    private void Update()
    {
        if (_timer <= 0f) return;
        _timer -= Time.deltaTime;
        Attract();
    }

    private void Attract()
    {
        List<ICollidable> list = AllManager.instance.CollisionManager.GetAllCollidables();
        Vector3 target = _player.transform.position;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (list[i] is ExpItem exp)
            {
                Vector3 dir = (target - exp.transform.position).normalized;
                exp.transform.position += dir * _speed * Time.deltaTime;
            }
        }
    }
}


