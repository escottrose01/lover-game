using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContainer : MonoBehaviour
{
    public Dialogue dialogue;

    public void ShowDialogue()
    {
        DialogueManager.Instance?.StartDialogue(dialogue);
    }
}
