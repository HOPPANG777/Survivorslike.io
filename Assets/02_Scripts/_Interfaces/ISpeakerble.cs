using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//==============================================================================
// ISpeakable
//==============================================================================
// 객체들의 사운드 관리를 위한 인터페이스
//
//==============================================================================
public interface ISpeakable
{
    // 사용할 사운드 이름
    string SoundName { get; }

    // 사용할 사운드 타입
    SoundType SoundType { get; }

    // 사용할 오디오 클립
    AudioClip SoundClip { get; }

    // 오디오를 출력할 주체
    AudioSource SoundSource { get; }

    // 사운드 출력
    void SoundPlay();

    // 사운드 출력 멈춤
    void SoundStop();
}

public enum SoundType
{ 
    NONE,
    BGM,
    SFX
}

