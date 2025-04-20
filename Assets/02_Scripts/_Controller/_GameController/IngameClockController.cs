using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class IngameClockController : MonoBehaviour 
{
    [TabGroup("Timer")][Tooltip("게임 타이머")][SerializeField] private TextMeshProUGUI timeCountText;

    public void UpdateTimer(float timeCount)
    {
        timeCountText.text = ((int)timeCount / 60 % 60).ToString("D2") + " : " + ((int)timeCount % 60).ToString("D2");
    }
}
