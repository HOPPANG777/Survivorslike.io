using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "MagnetItem", menuName = "ItemData/MagnetItemSO", order = 3)]
public class MagnetSO : ItemBaseSO
{
    [TabGroup("Magnet Item Data")][SerializeField][Tooltip("효과 지속 시간")]
    private float _duration = 3f;
    public float Duration { get { return _duration; } set { _duration = value; } }

    [TabGroup("Magnet Item Data")][SerializeField][Tooltip("젬 흡수 속도")]
    private float _speed = 20f;
    public float Speed { get { return _speed; } set { _speed = value; } }

    public override void Apply(GameObject user)
    {
        var magnet = user.GetComponent<IExpMagnet>();
        if (magnet != null)
        {
            magnet.StartMagnet(_duration, _speed);
        }
    }
}

