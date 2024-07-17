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

    public void PlayMusic()
    {
        backgroundMusic.Play();
    }
}