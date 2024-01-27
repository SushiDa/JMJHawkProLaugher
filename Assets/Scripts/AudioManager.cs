using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class AudioManager : SerializedMonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private List<AudioSource> SoloSFXSources;

    [SerializeField] private float FadeInTime;
    [SerializeField] private float FadeOutTime;

    [SerializeField] private Dictionary<string, ChanneledSFXSource> ChanneledSFXSourceList;
    [SerializeField] private List<string> PreLoadedClips;

    private float BPM;
    private int BeatsPerMeasure;
    private int SoloSFXSourceIndex;


    // Start is called before the first frame update
    void Awake()
    {
        AudioBridge.PlayCurrentMusicEvent += PlayCurrentMusic;
        AudioBridge.PlayMusicEvent += PlayMusic;
        AudioBridge.PauseMusicEvent += PauseMusic;
        AudioBridge.StopMusicEvent += StopMusic;
        AudioBridge.PlaySFXEvent += PlaySFX;
        AudioBridge.PlayChanneledSFXEvent += PlayChanneledSFX;
        AudioBridge.ChangeMusicPitchEvent += ChangeMusicPitch;

        foreach(var clip in PreLoadedClips)
        {
            AudioBridge.PreLoadClip(clip);
        }

        foreach(var channelEntry in ChanneledSFXSourceList)
        {
            if (channelEntry.Value.Sources == null) CreateSource(channelEntry.Value);
            AudioBridge.PreLoadClip(channelEntry.Key);
        }

        if (!AudioBridge.StreamingClipsLoaded)
        {
            AudioBridge.ClearCache();
            AudioBridge.LoadAllClipsFromStreamingAssets();
            AudioBridge.LoadAllIndividualClipsFromStreamingAssets("Music");
        }
    }

    private void OnDestroy()
    {
        AudioBridge.PlayCurrentMusicEvent -= PlayCurrentMusic;
        AudioBridge.PlayMusicEvent -= PlayMusic;
        AudioBridge.PauseMusicEvent -= PauseMusic;
        AudioBridge.StopMusicEvent -= StopMusic;
        AudioBridge.PlaySFXEvent -= PlaySFX;
        AudioBridge.PlayChanneledSFXEvent -= PlayChanneledSFX;
        AudioBridge.ChangeMusicPitchEvent -= ChangeMusicPitch;
    }

    private void Update()
    {
        if (BPM != 0)
        {
            float beatTime = 60 / BPM;
            float measureTime = beatTime * BeatsPerMeasure;
            float currentBeatTime = MusicSource.time % beatTime;
            float currentMeasureTime = MusicSource.time % measureTime;

            AudioBridge.CurrentBeatPercent = currentBeatTime / beatTime;
            AudioBridge.CurrentMeasurePercent = currentMeasureTime / measureTime;
            AudioBridge.BeatNumber = Mathf.FloorToInt(MusicSource.time / beatTime);
        }

        if(Keyboard.current.f2Key.wasPressedThisFrame)
        {
            AudioBridge.ClearCache(); 
            AudioBridge.LoadAllClipsFromStreamingAssets();
            AudioBridge.LoadAllIndividualClipsFromStreamingAssets("Music");
        }
    }

    private void PlayCurrentMusic()
    {
        if(MusicSource.clip != null)
            MusicSource.Play();
    }

    private void PlayMusic(AudioClip clip, float bpm, int beatsPreMesure)
    {
        if (clip == null)
        {
            StopMusic(true);
            MusicSource.clip = null;
        }

        MusicSource.clip = clip;
        MusicSource.time = 0;
        BPM = bpm;
        BeatsPerMeasure = beatsPreMesure;
        MusicSource.DOKill();
        MusicSource.volume = 0;
        MusicSource.DOFade(1, FadeInTime);
        PlayCurrentMusic();
    }
    private void PauseMusic()
    {
        MusicSource.Pause();
    }
    private void StopMusic(bool fade)
    {
        if (MusicSource.isPlaying)
        {
            MusicSource.DOKill();
            if (fade)
                MusicSource.DOFade(0, FadeOutTime).onComplete = () =>
                {
                    MusicSource.Stop();
                };
            else MusicSource.Stop();
        } else MusicSource.Stop();
    }

    private float _storedPitch;
    private void ChangeMusicPitch(float pitch)
    {
        if (pitch > 0)
        {
            _storedPitch = MusicSource.pitch;
            MusicSource.DOPitch(pitch,.2f);
        }
        else if (_storedPitch > 0)
        {
            MusicSource.DOPitch(_storedPitch,.2f);
            _storedPitch = -1;
        }
        else
        {
            MusicSource.DOPitch(1,.2f);
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        SoloSFXSources[0].PlayOneShot(clip);
    }

    private void PlayChanneledSFX(string clipName, AudioClip clip, bool applyModifiers)
    {
        if (ChanneledSFXSourceList.ContainsKey(clipName))
        {
            ChanneledSFXSource channel = ChanneledSFXSourceList[clipName];
            if (channel.Sources == null) CreateSource(channel);
            AudioSource currentSource = channel.Sources[channel.sourceIndex];
            if(clip != null && (!currentSource.isPlaying || currentSource.time >= channel.minimumPlayTime))
            {
                channel.sourceIndex = (channel.sourceIndex + 1) % channel.Sources.Length;
                AudioSource newSource = channel.Sources[channel.sourceIndex];
                newSource.DOKill();
                currentSource.DOKill();
                if (channel.QuickClip != null && channel.QuickClip != "" && currentSource.time >= channel.minimumPlayTime && currentSource.time <= channel.minimumPlayTime + channel.QuickMaximumPlayTime)
                {
                    var quickClip = AudioBridge.GetAudioClip(channel.QuickClip);
                    newSource.clip = quickClip == null ? clip : quickClip;
                }
                else
                {
                    newSource.clip = clip;
                }
                newSource.time = 0;
                if (applyModifiers)
                {
                    newSource.pitch = Random.Range(channel.PitchRange.x, channel.PitchRange.y);
                    newSource.volume = Random.Range(channel.VolumeRange.x, channel.VolumeRange.y);
                }
                else
                {
                    newSource.pitch = 1;
                    newSource.volume = 1;
                }
                newSource.Play();
                if (currentSource.isPlaying) currentSource.DOFade(0, channel.minimumPlayTime);

            }
        }
        else
        {
            AudioSource SoloSFXSource = SoloSFXSources[SoloSFXSourceIndex++ % SoloSFXSources.Count];
            SoloSFXSource.clip = clip;
            SoloSFXSource.time = 0;
            SoloSFXSource.Play();
        }
    }

    private void CreateSource(ChanneledSFXSource channeledSource)
    {
        AudioSource source = Instantiate(SoloSFXSources[0], transform);
        AudioSource source2 = Instantiate(SoloSFXSources[0], transform);
        channeledSource.Sources = new AudioSource[2] { source, source2 };
    }
}

[System.Serializable]
public class ChanneledSFXSource 
{
    internal AudioSource[] Sources;
    public float minimumPlayTime;
    public Vector2 PitchRange = new Vector2(1, 1);
    public Vector2 VolumeRange = new Vector2(1, 1);
    public string QuickClip;
    public float QuickMaximumPlayTime;
    public int sourceIndex;
}
