using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueContainer : MonoBehaviour
{
    public Dialogue dialogue;
    public UnityEvent dialogueClose;

    public void ShowDialogue()
    {
        DialogueManager.Instance?.StartDialogue(dialogue, dialogueClose);
    }
}
