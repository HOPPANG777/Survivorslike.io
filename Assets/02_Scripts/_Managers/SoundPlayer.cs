using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 실제 오디오 재생을 처리하는 객체
/// </summary>
public class SoundPlayer : MonoBehaviour, ISoundPlayer
{
    // 오디오 소스 컴포넌트
    [SerializeField] private AudioSource _source;

    /// <summary>재생에 사용되는 오디오 소스</summary>
    public AudioSource Source => _source;

    /// <summary>현재 재생 중인지 여부 반환</summary>
    public bool IsPlaying => _source != null && _source.isPlaying;

    /// <summary>
    /// 클립과 믹서 그룹을 설정하고 재생
    /// </summary>
    /// <param name="clip">재생할 오디오 클립</param>
    /// <param name="group">적용할 오디오 믹서 그룹</param>
    public void Play(AudioClip clip, AudioMixerGroup group)
    {
        if (_source == null) return;
        _source.clip = clip;
        _source.outputAudioMixerGroup = group;
        _source.Play();
    }
}
