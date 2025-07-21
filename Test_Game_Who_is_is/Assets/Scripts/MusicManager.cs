using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Main Music Tracks")]
    [SerializeField] private AudioClip mainTheme1;
    [SerializeField] private AudioClip mainTheme2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        SetVolume(0.5f);
    }
    public void PlayMainTheme1()
    {
        PlayLoopingTrack(mainTheme1);
    }

    
    public void PlayMainTheme2()
    {
        PlayLoopingTrack(mainTheme2);
    }

   
    public void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;
        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void PlayLoopingTrack(AudioClip clip)
    {
        if (clip == null) return;
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

   
    public void StopMusic() => _audioSource.Stop();
  
    public void SetVolume(float volume)
    {
        _audioSource.volume = Mathf.Clamp01(volume);
    }

    public bool IsPlaying() => _audioSource.isPlaying;
}
