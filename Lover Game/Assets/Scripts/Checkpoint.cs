using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    public Vector2 spawnOffset;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlatformerController>().collisions.below)
        {
            collision.GetComponent<Player>().checkpoint = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawCube(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position + (Vector3)spawnOffset, 0.5f * Vector3.one);
    }
}
