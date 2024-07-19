using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class PassLevel : MonoBehaviour
{
    [SerializeField] private Image passLevelImage; 
    private float duration = 2.0f; 

    public void FinishLevel()
    {
        if (passLevelImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = passLevelImage.color;
        color.a = 0;
        passLevelImage.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration);
            passLevelImage.color = color;
            yield return null;
        }

        color.a = 1;
        passLevelImage.color = color;
        PassScene();
    }

    private void PassScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}

