using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(DialogueContainer))]
public class DialogueQuest : Approachable, IQuest
{
    public Dialogue[] questDialogue;
    public UnityEvent questComplete;

    int questCompletionStatus;
    DialogueContainer dialogueContainer;
    Animator tooltipAnimator;

    bool isQuestComplete;

    private void Start()
    {
        dialogueContainer = GetComponent<DialogueContainer>();
        tooltipAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void ProgressQuest()
    {
        dialogueContainer.dialogue = questDialogue[questCompletionStatus++];

        if (questCompletionStatus == questDialogue.Length)
        {
            isQuestComplete = true;
            --questCompletionStatus;
        }
    }

    public void TryQuestComplete()
    {
        if (isQuestComplete)
        {
            questComplete.Invoke();
            isQuestComplete = false;
        }
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
