using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

public class WeaponSelectionController : MonoBehaviour, ISelectedIndex
{
    [TabGroup("ItemPanel")][Tooltip("무기 아이템 패널")][SerializeField] private GameObject _itemPanel;

    public void SelectedIndex(int index)
    {

    }
}
