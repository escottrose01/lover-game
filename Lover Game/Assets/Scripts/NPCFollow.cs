using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class NPCFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool halt;
    public float nearDistanceX = 2f;

    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = target.position + offset - transform.position;
        float distanceX = Mathf.Abs(distance.x);

        if (!halt && distanceX > nearDistanceX)
        {
            Vector2 directionalInput = Mathf.Clamp01(distanceX / 4f) * distance.normalized;
            player.SetDirectionalInput(directionalInput);
        }
        else player.SetDirectionalInput(Vector2.zero);
    }
}
