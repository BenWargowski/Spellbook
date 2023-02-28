using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider soundSlider;

    void Start()
    {
        // Set the starting value of the sound slider to the current volume of the audio source
        soundSlider.value = audioSource.volume;
    }

    void Update()
    {
        // Update the volume of the audio source based on the value of the sound slider
        audioSource.volume = soundSlider.value;
    }
}