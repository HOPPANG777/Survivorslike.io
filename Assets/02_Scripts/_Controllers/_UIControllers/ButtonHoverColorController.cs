using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverColorController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();
    [SerializeField] private Outline _outline;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _hoverColor;

    private readonly List<Color> _origTextColors = new List<Color>();
    private Color _origOutlineColor;

    private void Awake()
    {
        if (_texts.Count == 0)
        {
            var text = GetComponentInChildren<TextMeshProUGUI>();
            if (text != null) _texts.Add(text);
        }
        if (_outline == null) _outline = GetComponent<Outline>();

        foreach (var t in _texts)
            _origTextColors.Add(t != null ? t.color : Color.white);

        if (_outline != null) _origOutlineColor = _outline.effectColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var t in _texts)
            if (t != null) t.color = _hoverColor;
        if (_outline != null) _outline.effectColor = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < _texts.Count; i++)
            if (_texts[i] != null) _texts[i].color = _origTextColors[i];
        if (_outline != null) _outline.effectColor = _origOutlineColor;
    }
}