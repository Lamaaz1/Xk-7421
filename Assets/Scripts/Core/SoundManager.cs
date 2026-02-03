using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Clips")]
    public AudioClip matchClip;
    public AudioClip failClip;
    public AudioClip flipClip;

    [Header("Audio Sources")]
    public AudioSource sfxSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
    }

    public void PlayMatchSound()
    {
        if (matchClip != null)
            sfxSource.PlayOneShot(matchClip);
    }

    public void PlayFailSound()
    {
        if ( failClip != null)
            sfxSource.PlayOneShot(failClip);
    }

    public void PlayFlipSound()
    {
        if (flipClip != null)
            sfxSource.PlayOneShot(flipClip);
    }
}
