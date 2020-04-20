using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioClip[] stepClip;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip slide;
    public AudioClip damage;
    public AudioClip scream;
    public float pitchRange;
    private AudioSource audioSource;
    private float initialPitch;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        initialPitch = audioSource.pitch;
    }
    public void PlayStep()
    {
        if (audioSource.enabled)
        {
            int i = Random.Range(0, stepClip.Length);
            audioSource.clip = stepClip[i];
            audioSource.pitch = initialPitch + Random.Range(-pitchRange, +pitchRange);
            audioSource.Play();
        }
    }

    public void PlayJump()
    {
        if (audioSource.enabled)
        {
            audioSource.clip = jump;
            audioSource.pitch = initialPitch;
            audioSource.Play();
        }
    }
    public void PlayLand()
    {
        if (audioSource.enabled)
        {
            audioSource.clip = land;
            audioSource.Play();
        }
    }
    public void PlaySlide()
    {
        if (audioSource.enabled)
        {
            audioSource.clip = slide;
            audioSource.pitch = initialPitch;
            audioSource.Play();
        }
    }

    public void PlayDamage()
    {
        if (audioSource.enabled)
        {
            audioSource.clip = damage;
            audioSource.pitch = initialPitch;
            audioSource.Play();
        }
    }

    public void PlayScream()
    {
        if (audioSource.enabled)
        {
            audioSource.clip = scream;
            audioSource.pitch = initialPitch;
            audioSource.Play();
        }
    }
}
