using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class SliderPercentageDisplayController : MonoBehaviour
{
    [TabGroup("Slider and Text")][SerializeField] Slider _slider;
    [TabGroup("Slider and Text")][SerializeField] TextMeshProUGUI _percentageText; // TextMeshProUGUI를 사용하는 경우, 해당 타입으로 변경

    void Start()
    {
        if (_slider != null && _percentageText != null)
        {
            UpdatePercentage(_slider.value);
            _slider.onValueChanged.AddListener(UpdatePercentage);
        }
        else
        {
            Debug.LogError("슬라이더 또는 퍼센트 텍스트가 할당되지 않았습니다.");
        }
    }

    void UpdatePercentage(float value)
    {
        int percentage = Mathf.RoundToInt(value * 100);
        _percentageText.text = $"{percentage}%";
    }
}
