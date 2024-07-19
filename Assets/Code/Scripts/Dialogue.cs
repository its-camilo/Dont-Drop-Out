using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private string[] dialogueLines;

    private float typingTime = 0.05f;
    private int lineIndex;
    void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && lineIndex != 8 && lineIndex != 12 && lineIndex != 16)
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

        if (lineIndex == 8 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            SpecificDialogue(9);
        }

        if (lineIndex == 12 && Input.GetKeyDown(KeyCode.Space))
        {
            SpecificDialogue(13);
        }
    }

    private void StartDialogue()
    {
        lineIndex = 0;
        StartCoroutine(WriteLine());
    }

    private void SpecificDialogue(int dialogue)
    {
        lineIndex = dialogue;
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
