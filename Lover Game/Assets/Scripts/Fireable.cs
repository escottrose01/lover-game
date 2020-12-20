using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fireable : MonoBehaviour
{
    public float cooldownTime;

    protected Cooldown cooldown;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cooldown = new Cooldown(cooldownTime);
    }

    public abstract void Fire(Vector2 direction);
}
