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

    [SerializeField] private AudioSource sfxSource;

    public const string mixerMusic = "MusicVolume";
    public const string mixerSFX = "SFXVolume";

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Start()
    {
        float musicValue = 0f;
        float sfxValue = 0f;

        mixer.GetFloat(mixerMusic, out musicValue);
        mixer.GetFloat(mixerSFX, out sfxValue);

        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
    }

    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    private void SetMusicVolume(float value)
    {
        mixer.SetFloat(mixerMusic, value);
    }

    private void SetSFXVolume(float value)
    {
        mixer.SetFloat(mixerSFX, value);
    }
}