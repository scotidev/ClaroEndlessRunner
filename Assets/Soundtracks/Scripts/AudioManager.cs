using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private float globalMusicVolume = 1f;
    private float globalSfxVolume = .7f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null)
        {
            return;
        }

        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
        }

        musicSource.loop = true;
        musicSource.volume = globalMusicVolume;

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null)
        {
            return;
        }

        if (clip == null)
        {
            return;
        }

        float finalVolume = globalSfxVolume * volume;
        sfxSource.PlayOneShot(clip, finalVolume);
    }
}