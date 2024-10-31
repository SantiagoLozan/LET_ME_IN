using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voicesSlider;
    public AudioMixer audioMixer;
    public RectTransform panelOpciones;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        voicesSlider.value = PlayerPrefs.GetFloat("VoicesVolume", 0.5f);

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetVoicesVolume(voicesSlider.value);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        voicesSlider.onValueChanged.AddListener(SetVoicesVolume);
    }

    public void AbrirOpciones()
    {
        panelOpciones.gameObject.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.gameObject.SetActive(false);
    }

    public void SetMusicVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetVoicesVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("VoicesVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
        PlayerPrefs.SetFloat("VoicesVolume", volume);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}

