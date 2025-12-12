using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    public string nextSceneName = "MainScene";
    private const string VIDEO_FILE_NAME = "DesktopCutscene.mp4";
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        string videoURL = Path.Combine(Application.streamingAssetsPath, VIDEO_FILE_NAME);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        videoURL = "file:///" + videoURL.Replace('\\', '/');
#endif

        videoPlayer.url = videoURL;

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.errorReceived += (vp, message) =>
        {
            SceneManager.LoadScene(nextSceneName);
        };

        videoPlayer.prepareCompleted += (vp) =>
        {
            if (vp.targetTexture != null)
            {
                vp.Play();
            }
        };

        videoPlayer.Prepare();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}