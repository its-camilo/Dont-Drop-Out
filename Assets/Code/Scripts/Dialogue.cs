using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private AudioSource dialogueBlop, dialogueHuesos;
    [SerializeField] private GameObject blop, huesos;

    private float typingTime = 0.2f;
    private int lineIndex;
    void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            {
                if (dialogueText.text == dialogueLines[lineIndex])
                {
                    NextDialogueLine();
                }

                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[lineIndex];
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level2")
        {
            {
                if (dialogueText.text == dialogueLines[lineIndex])
                {
                    NextDialogueLine();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[lineIndex];
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level3")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (dialogueText.text == dialogueLines[lineIndex])
                {
                    NextDialogueLine();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[lineIndex];
                }
            }
        }
    }
    private void StartDialogue()
    {
        lineIndex = 0;
        StartCoroutine(WriteLine());
    }


    IEnumerator WriteLine()
    {
        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex].ToCharArray())
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void NextDialogueLine()
    {
        if (lineIndex < dialogueLines.Length - 1)
        {
            lineIndex++;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}