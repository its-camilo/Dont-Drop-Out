using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager is null");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        //
    }

    public bool IsPaused
    {
        get;
        set;
    }
}