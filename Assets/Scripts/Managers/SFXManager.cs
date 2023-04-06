using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip[] sfx;
    public AudioClip[] hitSFX;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameEvents.current.onHit += PlaySFX;
        GameEvents.current.onCriticalHit += PlaySFX;
        GameEvents.current.onGuard += PlaySFX;
        GameEvents.current.onCriticalGuard += PlaySFX;
        GameEvents.current.onDash += PlaySFX;
        GameEvents.current.onParry += PlaySFX;
    }

    private void PlaySFX(int soundFX)
    {
        if(soundFX == 0)
        {
            int randomHitSound = Random.Range(0, hitSFX.Length);
            source.Stop();
            source.clip = null;
            source.clip = hitSFX[randomHitSound];
            source.Play();
        }
        else
        {
            source.Stop();
            source.clip = null;
            source.clip = sfx[soundFX];
            source.Play();
        }
    }
}
