using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]
public class NightTransition : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public Transform player;

    public SpriteRenderer[] spritesToDarken;
    public Tilemap[] tilesToDarken;

    Animator animator;

    float maxDarkenAmt = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float t;
        if (player.position.x <= startPos.x) t = 0f;
        else if (player.position.x >= endPos.x) t = 1f;
        else t = (player.position.x - startPos.x) / (endPos.x - startPos.x);

        animator.Play("Background_NightTransition", 0, t);

        foreach (SpriteRenderer sr in spritesToDarken)
        {
            float darkenAmt = Mathf.Lerp(1f, maxDarkenAmt, t);
            sr.color = new Color(darkenAmt, darkenAmt, darkenAmt, 1f);
        }

        foreach (Tilemap tilemap in tilesToDarken)
        {
            float darkenAmt = Mathf.Lerp(1f, maxDarkenAmt, t);
            tilemap.color = new Color(darkenAmt, darkenAmt, darkenAmt, 1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPos, endPos);
    }
}
