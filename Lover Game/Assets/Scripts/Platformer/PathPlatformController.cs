using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlatformController : PlatformController
{
    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed;
    public bool cyclic;
    public float waitTime;

    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    public override void Start()
    {
        base.Start();
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; ++i)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    protected override Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime) return Vector3.zero;

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += (distanceBetweenWaypoints != 0) ?
            Time.deltaTime * speed / distanceBetweenWaypoints :
            1f;

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], Tween.GetEasedValue(percentBetweenWaypoints, Tween.Easing.InOutSine));

        if (percentBetweenWaypoints >= 1f)
        {
            percentBetweenWaypoints = 0f;
            ++fromWaypointIndex;

            if (!cyclic)
            {
                if (fromWaypointIndex == globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }

            nextMoveTime = Time.time + waitTime;
        }
        return newPos - transform.position;
    }

    void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 0.3f;
            int max = localWaypoints.Length;
            if (globalWaypoints != null) max = Mathf.Min(max, globalWaypoints.Length);
            for (int i = 0; i < max; ++i)
            {
                Vector3 globalWaypointPosition = (globalWaypoints != null) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPosition + Vector3.down * size, globalWaypointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPosition + Vector3.left * size, globalWaypointPosition + Vector3.right * size);
            }
        }
    }
}
