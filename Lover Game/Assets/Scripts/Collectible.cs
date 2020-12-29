using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    public UnityEvent onCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onCollect.Invoke();
            Destroy(transform.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawMesh(GetComponent<BoxCollider2D>().CreateMesh(false, false), transform.position);
    }
}