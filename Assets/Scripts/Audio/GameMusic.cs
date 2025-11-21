using UnityEngine;

public class GameMusic : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Músiques")]
    [SerializeField] private AudioClip _gameMainMusic;
    [SerializeField] private AudioClip _gameOverMusic;

    [Header("Volums")]
    [SerializeField] private float _mainMusicVolume = 0.8f;
    [SerializeField] private float _gameOverMusicVolume = 0.4f;

    private void OnEnable()
    {
        UI.GamePausedEvent += PlayMusic;
        PlayerCat.PlayerDeath += PlayGameOverMusic;
    }

    private void OnDisable()
    {
        UI.GamePausedEvent -= PlayMusic;
        PlayerCat.PlayerDeath -= PlayGameOverMusic;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.loop = true;
        _audioSource.volume = _mainMusicVolume;
        _audioSource.clip = _gameMainMusic;
        _audioSource.Play();
    }

    private void PlayMusic(bool pause)
    {
        if (pause)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.Play();
        }
    }

    private void PlayGameOverMusic()
    {
        _audioSource.Stop();

        _audioSource.volume = _gameOverMusicVolume;
        _audioSource.clip = _gameOverMusic;
        _audioSource.Play();
    }
}
