using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePlatformController : PlatformController
{
    public float speed;
    public float start;
    public float radius;
    float t;
    Vector3 center;

    public override void Start()
    {
        base.Start();
        center = transform.position - radius * new Vector3(Mathf.Cos(start * 2 * Mathf.PI), Mathf.Sin(start * 2 * Mathf.PI), 0f);
        t = start;
    }

    protected override Vector3 CalculatePlatformMovement()
    {
        float circumference = 2 * Mathf.PI * radius;
        t += Time.deltaTime * speed / circumference;
        t %= 1f;

        return center + radius * new Vector3(Mathf.Cos(t * 2 * Mathf.PI), Mathf.Sin(t * 2 * Mathf.PI), 0f) - transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 center = Application.isPlaying ? this.center : transform.position - radius * new Vector3(Mathf.Cos(start * 2 * Mathf.PI), Mathf.Sin(start * 2 * Mathf.PI), 0f);
        Gizmos.DrawWireSphere(center, radius);
    }
}
