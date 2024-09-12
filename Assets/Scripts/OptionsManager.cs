using UnityEngine;
using UnityEngine.Audio;  // Necesario para trabajar con AudioMixer
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioMixer audioMixer;

    public RectTransform panelOpciones;

    void Start()
    {
        musicSlider.value = 0.5f;
        sfxSlider.value = 0.5f;

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
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
    }

    public void SetSFXVolume(float volume)
    {
        float minVolume = 0.0001f;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(volume, minVolume)) * 20);
    }

}
