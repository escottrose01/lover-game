using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(DialogueContainer))]
public class DialogueQuest : Approachable, IQuest
{
    public Dialogue[] questDialogue;

    int questCompletionStatus;
    DialogueContainer dialogueContainer;
    Animator tooltipAnimator;

    private void Start()
    {
        dialogueContainer = GetComponent<DialogueContainer>();
        tooltipAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void ProgressQuest()
    {
        dialogueContainer.dialogue = questDialogue[questCompletionStatus];
        questCompletionStatus = Mathf.Min(questDialogue.Length - 1, questCompletionStatus + 1);
    }

    protected override void OnApproach()
    {
        tooltipAnimator.SetBool("Active", true);
    }

    protected override void OnStay()
    {
        if (PlayerInput.Interacting)
        {
            dialogueContainer.ShowDialogue();
        }
    }

    protected override void OnLeave()
    {
        tooltipAnimator.SetBool("Active", false);
    }
}
