using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("AudioManager is null");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        PlayMusic();
    }

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource fall;
    [SerializeField] private AudioSource clone;
    [SerializeField] private AudioSource damage1, damage2;
    [SerializeField] private AudioSource jump, jumpPlastic;
    [SerializeField] private AudioSource unclone;

    public void PlayMusic()
    {
        backgroundMusic.Play();
    }

    public void PlayFall()
    {
        fall.Play();
    }

    public void PlayClone()
    {
        clone.Play();
    }

    public void PlayDamage()
    {
        if (Random.value < 0.5f)
        {
            damage1.Play();
        }

        else
        {
            damage2.Play();
        }
    }

    public void PlayJump()
    {
        jump.Play();
    }

    public void PlayJumpPlastic()
    {
        jumpPlastic.Play();
    }

    public void PlayUnclone()
    {
        unclone.Play();
    }
}