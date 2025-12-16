using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    public string nextSceneName = "MainScene";
    private const string DESKTOP_VIDEO_NAME = "DesktopCutscene.mp4";
    private const string MOBILE_VIDEO_NAME = "MobileCutscene.mp4";
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        string videoFileName = MOBILE_VIDEO_NAME;

        if (!Application.isMobilePlatform)
        {
            videoFileName = DESKTOP_VIDEO_NAME;
        }

        if (videoPlayer == null)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        string videoURL = Path.Combine(Application.streamingAssetsPath, videoFileName);

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
            SkipCutscene();
            return;
        }

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                SkipCutscene();
            }
        }
    }

    private void SkipCutscene()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
        SceneManager.LoadScene(nextSceneName);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}