using UnityEngine;

public class GameMusic : MonoBehaviour
{
    private AudioSource _audioSource;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        UI.GamePausedEvent += PlayMusic;
    }

    private void PlayMusic(bool play)
    {
        if (play)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }
}
