using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPathPlatformController : PlatformController
{
    public PathData[] localPaths;
    Vector3[][] globalPaths;
    Vector3[] globalWaypoints;
    int curPath = 0;

    public float speed;
    public bool cyclic;
    public float waitTime;
    public bool waitForPlayer;

    int fromWaypointIndex;
    bool returning;
    float percentBetweenWaypoints;
    float nextMoveTime;
    bool clean = true;

    public override void Start()
    {
        base.Start();
        globalPaths = new Vector3[localPaths.Length][];
        for (int i = 0; i < localPaths.Length; ++i)
        {
            globalPaths[i] = new Vector3[localPaths[i].Length];
            for (int j = 0; j < localPaths[i].Length; ++j)
            {
                globalPaths[i][j] = localPaths[i][j] + transform.position;
            }
        }
        globalWaypoints = globalPaths[curPath];
    }

    protected override Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime) return Vector3.zero;

        fromWaypointIndex %= globalWaypoints.Length;
        if (percentBetweenWaypoints == 0f && fromWaypointIndex == 0 && !returning)
        {
            if (!clean)
            {
                clean = true;
                globalWaypoints = globalPaths[curPath];
            }
            if (waitForPlayer && !hasPassenger) return Vector3.zero;
        }

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
                    returning = !returning;
                }
            }

            nextMoveTime = Time.time + waitTime;
        }
        return newPos - transform.position;
    }

    public void SetPath(int index)
    {
        curPath = index % localPaths.Length;
        clean = false;
    }

    void OnDrawGizmosSelected()
    {
        if (localPaths != null)
        {
            for (int i = 0; i < localPaths.Length; ++i)
            {
                Vector3[] localPath = localPaths[i].path;
                float t = i / (float)localPaths.Length;
                Gizmos.color = t * Color.red + (1 - t) * Color.green;
                float size = 0.3f;
                int max = (globalPaths != null && globalPaths[i] != null) ? Mathf.Min(localPath.Length, globalPaths[i].Length) : localPath.Length;
                for (int j = 0; j < max; ++j)
                {
                    Vector3 globalWaypointPosition = (globalPaths != null && globalPaths[i] != null) ? globalPaths[i][j] : localPath[j] + transform.position;
                    Gizmos.DrawLine(globalWaypointPosition + Vector3.down * size, globalWaypointPosition + Vector3.up * size);
                    Gizmos.DrawLine(globalWaypointPosition + Vector3.left * size, globalWaypointPosition + Vector3.right * size);
                    if (curPath == i)
                    {
                        Gizmos.DrawLine(globalWaypointPosition + (Vector3.down + Vector3.left).normalized * size, globalWaypointPosition + (Vector3.up + Vector3.right).normalized * size);
                        Gizmos.DrawLine(globalWaypointPosition + (Vector3.down + Vector3.right).normalized * size, globalWaypointPosition + (Vector3.up + Vector3.left).normalized * size);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public struct PathData
    {
        [SerializeField]
        public Vector3[] path;

        public int Length
        {
            get { return path.Length; }
        }

        public Vector3 this[int i]
        {
            get { return path[i]; }
            set { path[i] = value; }
        }
    }
}
