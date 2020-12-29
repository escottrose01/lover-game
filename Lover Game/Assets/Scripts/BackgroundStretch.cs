using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class BackgroundStretch : MonoBehaviour
{
    public Camera backgroundCamera;
    public bool keepAspectRatio;

    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 curSize = sr.bounds.size;

        float targetHeight = 2f * backgroundCamera.orthographicSize;
        float targetWidth = backgroundCamera.aspect * targetHeight;

        float scaleX = targetWidth / curSize.x;
        float scaleY = targetHeight / curSize.y;

        if (keepAspectRatio)
        {
            if (scaleX > scaleY) scaleY = scaleX;
            else scaleX = scaleY;
        }

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
