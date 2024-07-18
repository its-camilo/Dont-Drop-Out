using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public Image progressBar;

    void Start()
    {
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");

        while (!operation.isDone)
        {
            progressBar.fillAmount = operation.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
