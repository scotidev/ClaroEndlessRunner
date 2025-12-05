using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundsSlider;
    [SerializeField] AudioMixer masterMixer;

    private void Start()
    {
        float savedVol = PlayerPrefs.GetFloat("SavedMasterVolume", 100f);
        soundsSlider.value = savedVol;
        SetVolume(savedVol);
    }

    public void SetVolume(float value)
    {
        // Salvar volume
        PlayerPrefs.SetFloat("SavedMasterVolume", value);

        float normalized = value / 100f;

        // Se for 0 → silêncio total
        if (normalized <= 0.0001f)
        {
            masterMixer.SetFloat("MasterVolume", -80f);
        }
        else
        {
            // Volume seguro (máximo = -2 dB para NÃO estourar)
            float volumeDB = Mathf.Lerp(-80f, -2f, normalized);
            masterMixer.SetFloat("MasterVolume", volumeDB);
        }
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundsSlider.value);
    }
}
