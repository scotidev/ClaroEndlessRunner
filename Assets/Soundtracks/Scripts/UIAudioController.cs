using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAudioController : MonoBehaviour, IPointerEnterHandler
{
    [Header("UI Audio Clips")]
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip hoverSFX;

    public static UIAudioController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayClickSFX()
    {
        if (AudioManager.Instance != null && Instance != null && Instance.clickSFX != null)
        {
            AudioManager.Instance.PlaySFX(Instance.clickSFX, 0.2f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Button button = GetComponent<Button>();
        if (button != null && !button.interactable) return;

        if (AudioManager.Instance != null && Instance != null && Instance.hoverSFX != null)
        {
            AudioManager.Instance.PlaySFX(Instance.hoverSFX, 0.2f);
        }
    }
}