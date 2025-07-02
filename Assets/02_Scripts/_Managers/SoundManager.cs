using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.SceneManagement;

public class SoundManager : SerializedSingleton<SoundManager>
{
    [TabGroup("Audio Mixer")][SerializeField] AudioMixer _audioMixer;
    [TabGroup("Mixer Groups")][SerializeField] AudioMixerGroup _masterMixerGroup;
    [TabGroup("Mixer Groups")][SerializeField] AudioMixerGroup _bgmMixerGroup;
    [TabGroup("Mixer Groups")][SerializeField] AudioMixerGroup _sfxMixerGroup;
    [TabGroup("Volum Sliders")][SerializeField] Slider _masterVolumeSlider;
    [TabGroup("Volum Sliders")][SerializeField] Slider _bgmVolumeSlider;
    [TabGroup("Volum Sliders")][SerializeField] Slider _sfxVolumeSlider;
    [TabGroup("Sound Dictionary")][SerializeField] Dictionary<string, List<ISpeakable>> _soundDictionary = new Dictionary<string, List<ISpeakable>>();

    protected override void Awake()
    {
        base.Awake();

        InitVolumSliders();
    }

    void InitVolumSliders()
    {
        if (_masterVolumeSlider == null) return;
        _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);

        if (_bgmVolumeSlider == null) return;
        _bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);

        if (_sfxVolumeSlider == null) return;
        _sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        // 초기 볼륨 설정 (세이브파일에서서 로드)
        float masterVolume = 1.0f;  //.GetFloat("MasterVolume", 1f);
        float musicVolume = 1.0f;   //.GetFloat("MusicVolume", 1f);
        float sfxVolume = 1.0f;     //.GetFloat("SFXVolume", 1f);
        SetMasterVolume(masterVolume);
        SetBGMVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        // 슬라이더 초기 값 설정
        if (_masterVolumeSlider != null) _masterVolumeSlider.value = masterVolume;
        if (_bgmVolumeSlider != null) _bgmVolumeSlider.value = musicVolume;
        if (_sfxVolumeSlider != null) _sfxVolumeSlider.value = sfxVolume;
    }

    // 사운드 등록 메서드드
    public void RegisterSound(string soundName, ISpeakable speaker, SoundType soundType)
    {
        if (soundType == SoundType.NONE) return;

        if (!_soundDictionary.ContainsKey(soundName))
        {
            _soundDictionary[soundName] = new List<ISpeakable>();
        }
            
        if (soundType == SoundType.BGM)
        {
            speaker.SoundSource.outputAudioMixerGroup = _bgmMixerGroup;
        }
        else if (soundType == SoundType.SFX)
        {
            speaker.SoundSource.outputAudioMixerGroup = _sfxMixerGroup;
        }

        if (_soundDictionary[soundName].Count < 6)
        {
            _soundDictionary[soundName].Add(speaker);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' has reached the maximum number of simultaneous instances (6).");
        }
    }

    public void UnregisterSound(string soundName, ISpeakable speaker)
    {
        if (_soundDictionary.ContainsKey(soundName))
        {
            _soundDictionary[soundName].Remove(speaker);
            if (_soundDictionary[soundName].Count == 0)
            {
                _soundDictionary.Remove(soundName);
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SoundManager.");
        }
    }

    // 사운드 재생 메서드
    public void PlaySound(string soundName)
    {
        if (_soundDictionary.ContainsKey(soundName))
        {
            List<ISpeakable> speakers = _soundDictionary[soundName];
            for (int i = 0; i < speakers.Count; ++i)
            {
                ISpeakable speaker = speakers[i];
                if (!speaker.SoundSource.isPlaying)
                {
                    speaker.SoundPlay();
                    return;
                }
            }
            // 모든 Speaker가 재생 중이라면 재생을 제한 (최대 6번)
            Debug.LogWarning($"Sound '{soundName}' is already playing 6 instances.");
        }
        else
        {
            Debug.LogError($"Sound '{soundName}' not found in SoundManager.");
        }
    }

    // 특정 Speaker의 사운드를 재생한다
    public void PlaySound(string soundName, ISpeakable speaker)
    {
        if (_soundDictionary.ContainsKey(soundName) && _soundDictionary[soundName].Contains(speaker))
        {
            if (!speaker.SoundSource.isPlaying)
            {
                speaker.SoundPlay();
            }
            else
            {
                // 다른 소스가 재생 중일 경우 기본 로직을 사용해 남는 소스를 찾는다
                PlaySound(soundName);
            }
        }
        else
        {
            Debug.LogWarning($"Speaker for sound '{soundName}' not registered. Using default PlaySound.");
            PlaySound(soundName);
        }
    }

    // 볼륨 조절 메서드
    public void SetBGMVolume(float volume)
    {
        // 로그 스케일로 변환 (AudioMixer는 로그 스케일을 사용)
        float dB = volume <= 0f ? -80f : Mathf.Log10(volume) * 20;
        _audioMixer.SetFloat("BGMVolume", dB);
        PlayerPrefs.SetFloat("BGM", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = volume <= 0f ? -80f : Mathf.Log10(volume) * 20;
        _audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFX", volume);
    }

    public void SetMasterVolume(float volume)
    {
        float dB = volume <= 0f ? -80f : Mathf.Log10(volume) * 20;
        _audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("Master", volume);
    }
}
