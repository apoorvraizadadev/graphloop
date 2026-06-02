using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlaySound(AudioClip sound)
    {
        if (audioSource == null)
        {
            audioSource = new GameObject("Audio").AddComponent<AudioSource>();
        }

        audioSource.PlayOneShot(sound);
    }

    public void SetPitch(float amount)
    {
        if (audioSource == null)
        {
            audioSource = new GameObject("Audio").AddComponent<AudioSource>();
        }

        audioSource.pitch = amount;
    }
}
