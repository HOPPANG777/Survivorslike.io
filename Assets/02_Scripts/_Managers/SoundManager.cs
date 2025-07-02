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

    [TabGroup("Sound Player")][SerializeField] SoundPlayer _playerPrefab; // 사운드 플레이어 프리팹
    [TabGroup("Sound Player")][SerializeField] int _initialPlayerCount = 6; // 초기 생성할 플레이어 수

    readonly List<SoundPlayer> _activePlayers = new List<SoundPlayer>(); // 사용 중인 플레이어 목록
    readonly Queue<SoundPlayer> _inactivePlayers = new Queue<SoundPlayer>(); // 대기 중인 플레이어 목록

    const int MAX_SIMULTANEOUS_SOUNDS = 6; // 최대 동시에 재생 가능한 사운드 수

    protected override void Awake()
    {
        base.Awake();

        CreatePlayers(_initialPlayerCount); // 초기 사운드 플레이어 생성
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

        if (!_soundDictionary[soundName].Contains(speaker))
        {
            _soundDictionary[soundName].Add(speaker);
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
            if (_activePlayers.Count >= MAX_SIMULTANEOUS_SOUNDS)
            {
                Debug.LogWarning($"Sound '{soundName}' is already playing {MAX_SIMULTANEOUS_SOUNDS} instances.");
                return;
            }

            List<ISpeakable> speakers = _soundDictionary[soundName];
            ISpeakable speaker = null;
            for (int i = 0; i < speakers.Count; i++)
            {
                if (speakers[i] != null)
                {
                    speaker = speakers[i];
                    break;
                }
            }
            if (speaker == null) return;

            var player = GetPlayer();
            var group = speaker.SoundType == SoundType.BGM ? _bgmMixerGroup : _sfxMixerGroup;
            player.Play(speaker.SoundClip, group);
        }
        else
        {
            Debug.LogError($"Sound '{soundName}' not found in SoundManager.");
        }
    }

    // 특정 Speaker의 사운드를 재생한다
    public void PlaySound(string soundName, ISpeakable speaker)
    {
        if (!_soundDictionary.ContainsKey(soundName))
        {
            _soundDictionary[soundName] = new List<ISpeakable>();
        }

        if (!_soundDictionary[soundName].Contains(speaker))
        {
            _soundDictionary[soundName].Add(speaker);
        }

        if (_activePlayers.Count >= MAX_SIMULTANEOUS_SOUNDS)
        {
            Debug.LogWarning($"Sound '{soundName}' is already playing {MAX_SIMULTANEOUS_SOUNDS} instances.");
            return;
        }

        var player = GetPlayer();
        var group = speaker.SoundType == SoundType.BGM ? _bgmMixerGroup : _sfxMixerGroup;
        player.Play(speaker.SoundClip, group);
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

    // 사운드 플레이어 초기화
    void CreatePlayers(int count)
    {
        if (_playerPrefab == null) return;
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(_playerPrefab, transform);
            obj.gameObject.SetActive(false);
            _inactivePlayers.Enqueue(obj);
        }
    }

    // 사용 가능한 플레이어 가져오기
    SoundPlayer GetPlayer()
    {
        if (_inactivePlayers.Count > 0)
        {
            var p = _inactivePlayers.Dequeue();
            _activePlayers.Add(p);
            p.gameObject.SetActive(true);
            return p;
        }

        var newPlayer = Instantiate(_playerPrefab, transform);
        _activePlayers.Add(newPlayer);
        return newPlayer;
    }

    // 플레이어 반환
    void ReleasePlayer(SoundPlayer player)
    {
        if (player == null) return;
        if (_activePlayers.Contains(player))
        {
            _activePlayers.Remove(player);
        }
        player.gameObject.SetActive(false);
        _inactivePlayers.Enqueue(player);
    }

    // 매 프레임 재생 상태 확인
    void Update()
    {
        for (int i = _activePlayers.Count - 1; i >= 0; i--)
        {
            var p = _activePlayers[i];
            if (!p.IsPlaying)
            {
                ReleasePlayer(p);
            }
        }
    }
}
