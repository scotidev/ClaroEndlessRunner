using UnityEngine;

public class SceneMusicLoader : MonoBehaviour
{
    [SerializeField] private AudioClip sceneMusic;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();

            if (sceneMusic != null)
            {
                AudioManager.Instance.PlayMusic(sceneMusic);
            }
        }
    }
}