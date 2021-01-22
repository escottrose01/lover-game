using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance { get { return instance; } }

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Animator animator;

    public int framesPerCharacter = 2;

    public bool Showing { get; private set; }

    Queue<string> sentences;

    string sentence;
    UnityEvent dialogueClose;
    IEnumerator dialogueTyper;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, UnityEvent dialogueClose)
    {
        sentences.Clear();
        Showing = true;

        this.dialogueClose = dialogueClose;

        animator.SetBool("isOpen", true);
        TimeManager.Instance.FreezeTime();

        nameText.text = dialogue.name;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Next();
    }

    public void Next()
    {
        if (sentence != null && dialogueText.text != sentence)
        {
            if (dialogueTyper != null) StopCoroutine(dialogueTyper);
            dialogueTyper = null;
            dialogueText.text = sentence;
            return;
        }
        else if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (dialogueTyper != null) StopCoroutine(dialogueTyper);
        sentence = sentences.Dequeue();
        dialogueTyper = TypeText();
        StartCoroutine(dialogueTyper);
    }

    IEnumerator TypeText()
    {
        // use a string builder?
        dialogueText.text = "";
        char[] chars = sentence.ToCharArray();
        int curChar = 0;
        int counter = 0;
        int size = sentence.Length;

        while (curChar < size)
        {
            if (PauseMenu.Instance == null || !PauseMenu.Instance.Paused)
            {
                counter = (counter + 1) % framesPerCharacter;
                if (counter == 0)
                {
                    dialogueText.text += chars[curChar++];
                    AudioManager.Instance.PlayDialogue();
                } 
            }
            yield return null;
        }

        dialogueTyper = null;
    }

    void EndDialogue()
    {
        if (dialogueTyper != null) StopCoroutine(dialogueTyper);
        dialogueTyper = null;
        animator.SetBool("isOpen", false);
        TimeManager.Instance.UnfreezeTime();
        sentence = null;
        Showing = false;

        dialogueClose.Invoke();
    }
}
