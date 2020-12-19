using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlatformerController target;
    public float verticalOffset;
    public float lookaheadDistanceX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public Vector2 focusAreaSize;

    FocusArea focusArea;

    float currentLookaheadX;
    float targetLookaheadX;
    float lookaheadDirX;
    float smoothLookVeloctiyX;
    float smoothVelocityY;

    bool lookaheadStopped;

    void Start()
    {
        focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(target.collider.bounds);

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0f)
        {
            lookaheadDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0f)
            {
                lookaheadStopped = false;
                targetLookaheadX = lookaheadDirX * lookaheadDistanceX;
            }
            else
            {
                if (!lookaheadStopped)
                {
                    targetLookaheadX = currentLookaheadX + (lookaheadDirX * lookaheadDirX - currentLookaheadX) / 4;
                    lookaheadStopped = true;
                }
            }
        }


        currentLookaheadX = Mathf.SmoothDamp(currentLookaheadX, targetLookaheadX, ref smoothLookVeloctiyX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * currentLookaheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        if (Application.isPlaying) Gizmos.DrawCube(focusArea.center, focusAreaSize);
        else Gizmos.DrawCube(transform.position, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 center;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0f;
            if (targetBounds.min.x < left) shiftX = targetBounds.min.x - left;
            else if (targetBounds.max.x > right) shiftX = targetBounds.max.x - right;
            left += shiftX;
            right += shiftX;

            float shiftY = 0f;
            if (targetBounds.min.y < bottom) shiftY = targetBounds.min.y - bottom;
            else if (targetBounds.max.y > top) shiftY = targetBounds.max.y - top;
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
