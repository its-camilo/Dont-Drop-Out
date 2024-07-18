using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        GameManager.Instance.IsPaused = false;
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.Instance.IsPaused)
            {
                GameManager.Instance.IsPaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }

            else if (GameManager.Instance.IsPaused)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.Instance.IsPaused = false;
        pauseMenu.SetActive(false);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.IsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}