using UnityEngine;

//==============================================================================
// UnitBase
//==============================================================================
// UnitBase 스크립트
//	
//==============================================================================

public class UnitBase : MonoBehaviour, ITeamMember, ISpeakable
{
    [Header("Unit Base")]
    [Tooltip("Unit Data")][SerializeField] private UnitBaseSO _unitBaseData;
    public UnitBaseSO UnitBaseData { get { return _unitBaseData; } }

    public TeamType TeamType { get { return _unitBaseData.TeamType; } set { _unitBaseData.TeamType = value; } }

    [Header("Unit Sound")]
    public string SoundName { get { return _unitBaseData.UnitSpeaker.SoundName; } }

    public SoundType SoundType { get { return _unitBaseData.UnitSpeaker.SoundType; } }

    public AudioClip SoundClip { get { return _unitBaseData.UnitSpeaker.SoundClip; } }

    public AudioSource SoundSource { get { return _unitBaseData.UnitSpeaker.SoundSource; } }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SoundPlay()
    {

    }

    public void SoundStop()
    {
       
    }
}
