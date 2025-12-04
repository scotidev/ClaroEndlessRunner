using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogueObj;
    public Text actorNameText;
    public Text speetchText;

    [Header("Variables")]
    public float typingSpeed;
    private string[] sentences;
    private int index;
    private Coroutine typingCoroutine;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            NextSentence();
        }
    }
    public void Speetch(string[] txt, string actorName)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogueObj.SetActive(true);
        speetchText.text = "";
        actorNameText.text = actorName;
        sentences = txt;
        index = 0;
        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        speetchText.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            speetchText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    public void NextSentence()
    {
        if (speetchText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                speetchText.text = "";
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                typingCoroutine = StartCoroutine(TypeSentence());
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void HidePanel()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        speetchText.text = "";
        actorNameText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
    }

    public void EndDialogue()
    {
        speetchText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
    }
}
