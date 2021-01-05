using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floatable : MonoBehaviour
{
    public float offsetY = 0.25f;

    bool cycle;
    float movementPercent;
    float speed = 0.5f;
    Vector2 startPos;
    Vector2 bottom, top;

    void Start()
    {
        startPos = transform.position;
        bottom = startPos + offsetY * Vector2.down;
        top = startPos + offsetY * Vector2.up;
        movementPercent = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = 2 * offsetY;
        movementPercent += Time.deltaTime * speed / distance;

        if (cycle) transform.position = Vector2.Lerp(top, bottom, Tween.GetEasedValue(movementPercent, Tween.Easing.InOutQuad));
        else transform.position = Vector2.Lerp(bottom, top, Tween.GetEasedValue(movementPercent, Tween.Easing.InOutQuad));

        if (movementPercent >= 1f)
        {
            movementPercent = 0f;
            cycle = !cycle;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Application.isPlaying) Gizmos.DrawLine(startPos + offsetY * Vector2.up, startPos + offsetY * Vector2.down);
        else Gizmos.DrawLine((Vector2)transform.position + offsetY * Vector2.up, (Vector2)transform.position + offsetY * Vector2.down);
    }
}
