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
            progressBar.rectTransform.localScale = new Vector3(1f, operation.progress, 1f);
            yield return new WaitForEndOfFrame();
        }
    }
}
