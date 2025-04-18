using UnityEngine;

public class PlayerUnit : UnitBase, IPlayerAttribute
{
    [Header("Player")]
    [Tooltip("Player ´É·ÂÄ¡")][SerializeField] private PlayerableAttribute _attributes;
    public PlayerableAttribute Attributes { get { return _attributes; } set { _attributes = value; } }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
