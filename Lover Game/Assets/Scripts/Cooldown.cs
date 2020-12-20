using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown
{
    float cooldownTime;
    float readyTime;

    public Cooldown(float cooldownTime)
    {
        this.cooldownTime = cooldownTime;
        readyTime = Time.time;
    }

    public void StartCooldown()
    {
        readyTime = Time.time + cooldownTime;
    }

    public bool CheckReady()
    {
        return Time.time >= readyTime;
    }
}
