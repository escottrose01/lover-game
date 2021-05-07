using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openPos;
    public float speed;

    Vector3 globalClosedPos, globalOpenPos;
    bool opening = false;
    float percentageCompleted = 1f;

    // Start is called before the first frame update
    void Start()
    {
        globalOpenPos = transform.position + openPos;
        globalClosedPos = transform.position;

        if (speed == 0) speed = float.PositiveInfinity;
    }

    private void Update()
    {
        if (percentageCompleted < 1f)
        {
            float dist = Vector2.Distance(globalClosedPos, globalOpenPos);
            percentageCompleted += (dist > 0) ?
                Time.deltaTime * speed / dist :
                1f;

            if (opening) transform.position = Vector3.Lerp(globalClosedPos, globalOpenPos, Tween.GetEasedValue(percentageCompleted, Tween.Easing.InOutSine));
            else transform.position = Vector3.Lerp(globalOpenPos, globalClosedPos, Tween.GetEasedValue(percentageCompleted, Tween.Easing.InOutSine));
        }
    }

    public void OpenDoor()
    {
        if (!opening)
        {
            opening = true;
            percentageCompleted = 1f - percentageCompleted;
        }
    }

    public void CloseDoor()
    {
        if (opening)
        {
            opening = false;
            percentageCompleted = 1f - percentageCompleted;
        }
    }

    void OnDrawGizmos()
    {
        float size = 0.3f;
        Gizmos.color = Color.red;
        Vector3 curOpenPos = (Application.isPlaying) ? globalOpenPos : transform.position + openPos;
        Vector3 curClosedPos = (Application.isPlaying) ? globalClosedPos : transform.position;

        Gizmos.DrawLine(curOpenPos + Vector3.down * size, curOpenPos + Vector3.up * size);
        Gizmos.DrawLine(curOpenPos + Vector3.left * size, curOpenPos + Vector3.right * size);

        Gizmos.DrawLine(curClosedPos + Vector3.down * size, curClosedPos + Vector3.up * size);
        Gizmos.DrawLine(curClosedPos + Vector3.left * size, curClosedPos + Vector3.right * size);
    }
}
