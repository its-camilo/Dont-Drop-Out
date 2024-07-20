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
            if (lineIndex == 1 || lineIndex == 3 || lineIndex == 4 || lineIndex == 5 || lineIndex == 7 || lineIndex == 9 || lineIndex == 11 || lineIndex == 13 || lineIndex == 14 || lineIndex == 17 || lineIndex == 21 || lineIndex == 23) //huesos
            {
                dialogueBlop.Stop();
                blop.SetActive(false);
                if (!dialogueHuesos.isPlaying)
                {
                    dialogueHuesos.Play();
                    huesos.SetActive(true);
                }
            }

            else if (lineIndex == 0 || lineIndex == 2 || lineIndex == 6 || lineIndex == 10 || lineIndex == 15 || lineIndex == 20) //blop
            {
                huesos.SetActive(false);
                dialogueHuesos.Stop();
                if (!dialogueBlop.isPlaying)
                {
                    blop.SetActive(true);
                    dialogueBlop.Play();
                }
            }

            else
            {
                huesos.SetActive(false);
                blop.SetActive(false);
                dialogueHuesos.Stop();
                dialogueBlop.Stop();
            }

            if (Input.GetKeyDown(KeyCode.F) && lineIndex != 8 && lineIndex != 12 && lineIndex != 16 && lineIndex != 19 && lineIndex != 20 && lineIndex != 22)
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (dialogueText.text == dialogueLines[lineIndex])
                {
                    NextDialogueLine();
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "Level2")
        {
            if (lineIndex == 0 || lineIndex == 1 || lineIndex == 2 || lineIndex == 5 || lineIndex == 7 || lineIndex == 9 || lineIndex == 10) //huesos
            {
                blop.SetActive(false);
                dialogueBlop.Stop();
                if (!dialogueHuesos.isPlaying)
                {
                    huesos.SetActive(true);
                    dialogueHuesos.Play();
                }
            }

            else if (lineIndex == 3 || lineIndex == 4) //blop
            {
                huesos.SetActive(false);
                dialogueHuesos.Stop();
                if (!dialogueBlop.isPlaying)
                {
                    blop.SetActive(true);
                    dialogueBlop.Play();
                }
            }

            else
            {
                huesos.SetActive(false);
                blop.SetActive(false);
                dialogueHuesos.Stop();
                dialogueBlop.Stop();
            }

            if (Input.GetKeyDown(KeyCode.F) && lineIndex != 3 && lineIndex != 6 && lineIndex != 8 && lineIndex != 9)
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

        if (SceneManager.GetActiveScene().name == "Level3")
        {
            if (lineIndex == 1 ||lineIndex == 2 || lineIndex == 3 || lineIndex == 5 || lineIndex == 6 || lineIndex == 8 || lineIndex == 9 || lineIndex == 10 || lineIndex == 11) //huesos
            {
                blop.SetActive(false);
                dialogueBlop.Stop();
                if (!dialogueHuesos.isPlaying)
                {
                    huesos.SetActive(true);
                    dialogueHuesos.Play();
                }
            }

            else if (lineIndex == 0) //blop
            {
                huesos.SetActive(false);
                dialogueHuesos.Stop();
                if (!dialogueBlop.isPlaying)
                {
                    blop.SetActive(true);
                    dialogueBlop.Play();
                }
            }

            else
            {
                huesos.SetActive(false);
                blop.SetActive(false);
                dialogueHuesos.Stop();
                dialogueBlop.Stop();
            }

            if (Input.GetKeyDown(KeyCode.F) && lineIndex != 2 && lineIndex != 4 && lineIndex != 7 && lineIndex != 8 && lineIndex != 10)
            {
                if (dialogueText.text == dialogueLines[lineIndex])
                {
                    NextDialogueLine();
                }

                //else
                //{
                //    StopAllCoroutines();
                //    dialogueText.text = dialogueLines[lineIndex];
                //}
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