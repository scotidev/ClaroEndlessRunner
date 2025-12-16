using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
    public GameObject desktopCanvasRoot;
    public GameObject mobileCanvasRoot;

    void Awake()
    {
        bool isMobile = Application.isMobilePlatform;

        if (isMobile)
        {
            if (mobileCanvasRoot != null) mobileCanvasRoot.SetActive(true);
            if (desktopCanvasRoot != null) desktopCanvasRoot.SetActive(false);
        }
        else
        {
            if (mobileCanvasRoot != null) mobileCanvasRoot.SetActive(false);
            if (desktopCanvasRoot != null) desktopCanvasRoot.SetActive(true);
        }
    }
}