using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using UnityEngine;


public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Serializable]
    public class ClipSettings
    {
        public AudioClipType type;
        public AudioClip clip;
    }
    
    
    private const string MusicEnabledKey = "MusicEnabled";
    private const string SoundEnabledKey = "SoundEnabled";
    private const string SoundVolumeKey = "SoundVolume";

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource continuousSource;
    [SerializeField] private Transform soundSourcesRoot;
    [Space]
    [SerializeField] private List<ClipSettings> clipSettings;

    private List<AudioSource> _soundSources = new List<AudioSource>();
    private float _soundVolume;
    private bool _isMusicEnabled;
    private bool _isSoundEnabled;


    public float SoundVolume => _soundVolume;


    public bool IsMusicEnabled => _isMusicEnabled;


    public bool IsSoundEnabled => _isSoundEnabled;


    protected override void Awake()
    {
        base.Awake();

        _soundSources = soundSourcesRoot.GetComponentsInChildren<AudioSource>().ToList();
        
        LoadPrefs();

        PlayMusic();
    }


    public void PlayMusic()
    {
        musicSource.Play();
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }


    public void PlaySound(AudioClipType type)
    {
        var clipSetting = clipSettings.FirstOrDefault(setting => setting.type == type);

        if (clipSetting != null &&
            clipSetting.clip != null &&
            TryGetAvailableSoundSource(out var outSoundSource))
        {
            outSoundSource.PlayOneShot(clipSetting.clip);
        }
    }


    public void StartPlaying(AudioClipType type)
    {
        var clipSetting = clipSettings.FirstOrDefault(setting => setting.type == type);

        if (clipSetting != null &&
            clipSetting.clip != null)
        {
            continuousSource.clip = clipSetting.clip;
            continuousSource.Play();
        }
    }


    public void StopPlaying(AudioClipType type)
    {
        var clipSetting = clipSettings.FirstOrDefault(setting => setting.type == type);

        if (clipSetting != null &&
            clipSetting.clip != null &&
            continuousSource.clip == clipSetting.clip)
        {
            continuousSource.Stop();
        }
    }


    public void SetVolume(float value)
    {
        _soundVolume = Mathf.Clamp01(value);
        
        continuousSource.volume = _soundVolume;

        foreach (var soundSource in _soundSources)
        {
            soundSource.volume = _soundVolume;
        }
        
        SavePrefs();
    }


    public void SetMusic(bool isEnabled)
    {
        if (isEnabled == _isMusicEnabled) return;
        
        SetMusicSource(isEnabled);
        SavePrefs();
    }


    public void SetSound(bool isEnabled)
    {
        if (isEnabled == _isSoundEnabled) return;

        SetSoundSources(isEnabled);
        SavePrefs();
    }


    private void SetMusicSource(bool isEnabled)
    {
        _isMusicEnabled = isEnabled;
        musicSource.mute = !_isMusicEnabled;
    }
    
    
    private void SetSoundSources(bool isEnabled)
    {
        _isSoundEnabled = isEnabled;

        foreach (var soundSource in _soundSources)
        {
            soundSource.mute = !_isSoundEnabled;
        }
    }
    
    
    private bool TryGetAvailableSoundSource(out AudioSource outSource)
    {
        outSource = null;
        
        foreach (var soundSource in _soundSources)
        {
            if (!soundSource.isPlaying)
            {
                outSource = soundSource;
                return true;
            }
        }

        return false;
    }


    private void SavePrefs()
    {
        int isMusic = Convert.ToInt32(_isMusicEnabled);
        int isSound = Convert.ToInt32(_isSoundEnabled);

        PlayerPrefs.SetFloat(SoundVolumeKey, _soundVolume);
        PlayerPrefs.SetInt(MusicEnabledKey, isMusic);
        PlayerPrefs.SetInt(SoundEnabledKey, isSound);
        PlayerPrefs.Save();
    }


    private void LoadPrefs()
    {
        float soundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, .5f);
        int isMusic = PlayerPrefs.GetInt(MusicEnabledKey, 1);
        int isSound = PlayerPrefs.GetInt(SoundEnabledKey, 1);

        _soundVolume = soundVolume;
        _isMusicEnabled = isMusic != 0;
        _isSoundEnabled = isSound != 0;
        
        SetMusicSource(_isMusicEnabled);
        SetSoundSources(_isSoundEnabled);
    }
}
