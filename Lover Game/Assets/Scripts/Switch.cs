using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private Animator animator;
    bool triggered = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateSwitch()
    {
        if (!triggered)
        {
            triggered = true;
            animator.SetBool("Active", true);
            AudioManager.Instance.PlaySwitchToggle();
        }
    }

    public void DeactivateSwitch()
    {
        triggered = false;
        animator.SetBool("Active", false);
    }

    public void ToggleSwitch()
    {
        if (triggered) DeactivateSwitch();
        else ActivateSwitch();
    }
}
