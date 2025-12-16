using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Volumes Globais")]
    [SerializeField] private float globalMusicVolume = 0.05f;
    [SerializeField] private float globalSfxVolume = 0.2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource != null)
            {
                musicSource.volume = 1f;
            }
            if (sfxSource != null)
            {
                sfxSource.volume = 1f;
            }

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
        if (sfxSource == null || clip == null)
        {
            return;
        }

        float finalVolume = globalSfxVolume * volume;

        sfxSource.PlayOneShot(clip, finalVolume);
    }
}