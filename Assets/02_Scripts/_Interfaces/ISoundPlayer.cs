using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 사운드 재생을 담당하는 플레이어 인터페이스
/// </summary>
public interface ISoundPlayer
{
    /// <summary>재생에 사용되는 오디오 소스</summary>
    AudioSource Source { get; }

    /// <summary>사운드 클립과 믹서 그룹을 설정하고 재생</summary>
    void Play(AudioClip clip, AudioMixerGroup group);

    /// <summary>현재 재생 중인지 여부</summary>
    bool IsPlaying { get; }
}
