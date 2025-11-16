using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioClipType
{
    GravitySwitch,
    GamePaused,
    Meow
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource _audioSource;
    private AudioSource _newAudioSource;

    public List<AudioClip> Clips = new List<AudioClip>();
    public Dictionary<AudioClipType, AudioClip> ClipList = new Dictionary<AudioClipType, AudioClip>();

    private void Awake()
    {
        Instance = this;

        _audioSource = GetComponent<AudioSource>();
        _newAudioSource = gameObject.AddComponent<AudioSource>();

        ClipList.Add(AudioClipType.GravitySwitch, Clips[0]);
        ClipList.Add(AudioClipType.GamePaused, Clips[1]);
        ClipList.Add(AudioClipType.Meow, Clips[2]);
    }

    public void PlayClip(AudioClipType clipType)
    {
        if (!_audioSource.isPlaying)
        {
            if (ClipList.TryGetValue(clipType, out AudioClip clip))
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }
        else
        {
            if (ClipList.TryGetValue(clipType, out AudioClip clip))
            {
                _newAudioSource.clip = clip;
                _newAudioSource.Play();
            }
        }
    }
}
