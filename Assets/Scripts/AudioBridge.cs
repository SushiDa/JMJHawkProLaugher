
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class AudioBridge
{
    internal static event System.Action<AudioClip, float, int> PlayMusicEvent;
    internal static event System.Action PlayCurrentMusicEvent;
    internal static event System.Action PauseMusicEvent;
    internal static event System.Action<bool> StopMusicEvent;
    internal static event System.Action<float> ChangeMusicPitchEvent;
    internal static event System.Action<AudioClip> PlaySFXEvent;
    internal static event System.Action<string, AudioClip, bool> PlayChanneledSFXEvent;

    static Dictionary<string, List<AudioClip>> LoadedClips = new Dictionary<string, List<AudioClip>>();
    static Dictionary<string, List<AudioClip>> StreamingClips = new Dictionary<string, List<AudioClip>>();

    internal static float CurrentBeatPercent;
    internal static float CurrentMeasurePercent;
    internal static int BeatNumber;

    internal static float CurrentBPM;
    internal static float CurrentBPMesure;

    internal static bool StreamingClipsLoaded = false;

    private static AudioClip GetClip(string clipName, bool fromStreamingAssets)
    {
        if (clipName == null || clipName == "") return null;
        if (fromStreamingAssets) return null;
        AudioClip result = null;
        Dictionary<string, List<AudioClip>> clipDictionary = fromStreamingAssets ? StreamingClips : LoadedClips;

        if (!clipDictionary.ContainsKey(clipName))
        {
            if (fromStreamingAssets)
            {
                string path = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Audio"), clipName);
                clipDictionary.Add(clipName, new List<AudioClip>());
                if (File.Exists(path + ".wav"))
                {
                    LoadClipFromStreamingAsset(path + ".wav", clipName);
                }
                else if(Directory.Exists(path))
                {
                    foreach (var file in Directory.GetFiles(path, "*.wav", new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive }))
                    {
                        LoadClipFromStreamingAsset(file, clipName);
                    }
                }
            }
            else
            {
                List<AudioClip> clips = new List<AudioClip>();
                clips.AddRange(Resources.LoadAll<AudioClip>("Audio/" + clipName));
                clipDictionary.Add(clipName, clips);
            }
        }

        if (clipDictionary[clipName].Count > 0)
        {
            result = clipDictionary[clipName][Random.Range(0, clipDictionary[clipName].Count)];
        }

        return result;
    }

    internal static void ClearCache()
    {
        StreamingClips.Clear();
    }
    internal static void PreLoadClip(string clipName)
    {
        if (clipName == null || clipName == "") return;
        GetClip(clipName, false);
    }
    internal static void PlayMusic(string clipName, float bpm = 120, int beatsPerMesure = 4)
    {
        if (clipName == null || clipName == "") return;
        AudioClip clip = GetAudioClip(clipName);
        CurrentBPM = bpm;
        CurrentBPMesure = beatsPerMesure;
        if(clip != null)
            PlayMusicEvent?.Invoke(clip, bpm, beatsPerMesure);
    }
    internal static void PlayCurrentMusic()
    {
        PlayCurrentMusicEvent?.Invoke();
    }
    internal static void PauseMusic()
    {
        PauseMusicEvent?.Invoke();
    }
    internal static void StopMusic(bool fade = true)
    {
        StopMusicEvent?.Invoke(fade);
    }
    internal static void ChangeMusicPitch(float pitch)
    {
        ChangeMusicPitchEvent?.Invoke(pitch);
    }
    internal static void PlaySFX(string clipName)
    {
        if (clipName == null || clipName == "") return;
        AudioClip clip = GetAudioClip(clipName);
        if (clip != null)
            PlaySFXEvent?.Invoke(clip);
    }
    internal static void PlayChanneledSFX(string clipName)
    {
        if (clipName == null || clipName == "") return;
        bool applyModifiers = false;
        AudioClip clip = GetClip(clipName, true);
        if (clip == null)
        {
            clip = GetClip(clipName, false);
            applyModifiers = true;
        }
        if (clip != null)
            PlayChanneledSFXEvent?.Invoke(clipName, clip, applyModifiers);
    }

    internal static AudioClip GetAudioClip(string clipName)
    {
        if (clipName == null || clipName == "") return null;
        AudioClip clip = GetClip(clipName, true);
        if (clip == null) clip = GetClip(clipName, false);
        return clip;
    }

    internal static float BeatPercentToMs(float percent)
    {
        float beatTime = 60 / CurrentBPM;
        return beatTime * percent;
    }
    internal static float MesurePercentToMs(float percent)
    {
        float mesureTime = (60 / CurrentBPM) * CurrentBPMesure;
        return mesureTime * percent;
    }

    internal static void LoadAllClipsFromStreamingAssets()
    {
        StreamingClipsLoaded = true;
        string path = Path.Combine(Application.streamingAssetsPath, "Audio");
        if (Directory.Exists(path))
        {
            foreach(var directory in Directory.GetDirectories(path))
            {
                string clipName = directory.Replace(path, "");
                if(clipName.StartsWith("/") || clipName.StartsWith("\\"))
                {
                    clipName = clipName.Remove(0, 1);
                }
                GetClip(clipName, true);
            }
        }
    }

    internal static void LoadAllIndividualClipsFromStreamingAssets(string baseClipName)
    {
        string path = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Audio"), baseClipName);


        if (Directory.Exists(path))
        {
            foreach (var file in Directory.GetFiles(path, "*.wav", new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive }))
            {
                string clipName = file.Replace(Path.Combine(Application.streamingAssetsPath, "Audio"), "").Replace(".wav", "").Replace("\\","/");
                if(clipName.StartsWith("/"))
                {
                    clipName = clipName.Remove(0, 1);
                }
                GetClip(clipName, true);
            }
        }
    }

    static async Task LoadClipFromStreamingAsset(string path, string clipName)
    {
        if (clipName == null || clipName == "") return;
        AudioClip clip = null;
        string filePath = "file://" + path;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log($"{uwr.error}");
                }
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
                }
            }
            catch (System.Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        if(clip != null && StreamingClips.ContainsKey(clipName))
        {
            StreamingClips[clipName].Add(clip);
        }
    }

}

[System.Serializable]
public struct MusicInfo
{
    public string clipName;
    public float BPM;
    public int BeatsPerMesure;
    public int Level;
}
