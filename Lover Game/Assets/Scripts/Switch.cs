using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    private Animator animator;
    bool triggered = false;

    public bool startActive;
    public UnityEvent onActivate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (startActive)
        {
            triggered = true;
            animator.SetBool("Active", true);
            onActivate.Invoke();
        }
    }

    public void ActivateSwitch()
    {
        if (!triggered)
        {
            triggered = true;
            animator.SetBool("Active", true);
            AudioManager.Instance.PlaySwitchToggle();
            onActivate.Invoke();
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
