using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SoundUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    public const string mixerMusic = "MusicVolume";
    public const string mixerSFX = "SFXVolume";
    public const string mixerMusicToggled = "MusicToggled";
    public const string mixerSFXToggled = "SFXToggle";

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        musicToggle.onValueChanged.AddListener(SetMusicToggle);
        sfxToggle.onValueChanged.AddListener(SetSFXToggle);
    }

    void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat(mixerMusic, 0f);
        float sfxVolume = PlayerPrefs.GetFloat(mixerSFX, 0f);

        bool musicIsToggled = PlayerPrefs.GetInt(mixerMusicToggled, 1) == 1 ? true : false;
        bool sfxIsToggled = PlayerPrefs.GetInt(mixerSFXToggled, 1) == 1 ? true : false;

        SetMusicVolume(musicVolume);
        musicSlider.value = musicVolume;

        SetSFXVolume(sfxVolume);
        sfxSlider.value = sfxVolume;

        SetMusicToggle(musicIsToggled);
        musicToggle.isOn = musicIsToggled;

        SetSFXToggle(sfxIsToggled);
        sfxToggle.isOn = sfxIsToggled;
    }

    private void SetMusicVolume(float value)
    {
        mixer.SetFloat(mixerMusic, value);
        PlayerPrefs.SetFloat(mixerMusic, value);

        SetMusicToggle(true);
        musicToggle.isOn = true;
    }

    private void SetSFXVolume(float value)
    {
        mixer.SetFloat(mixerSFX, value);
        PlayerPrefs.SetFloat(mixerSFX, value);

        SetSFXToggle(true);
        sfxToggle.isOn = true;
    }

    private void SetMusicToggle(bool isToggled)
    {
        if (isToggled)
            mixer.SetFloat(mixerMusic, musicSlider.value);
        else
            mixer.SetFloat(mixerMusic, -80f);

        PlayerPrefs.SetInt(mixerMusicToggled, isToggled ? 1 : 0);
    }

    private void SetSFXToggle(bool isToggled)
    {
        if (isToggled)
            mixer.SetFloat(mixerSFX, sfxSlider.value);
        else
            mixer.SetFloat(mixerSFX, -80f);

        PlayerPrefs.SetInt(mixerSFXToggled, isToggled ? 1 : 0);
    }
}