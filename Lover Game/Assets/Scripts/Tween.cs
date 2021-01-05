using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public static class Tween
{
    const float c1 = 1.70158f;
    const float c2 = c1 * 1.525f;
    const float c3 = c1 + 1f;
    const float c4 = (2 * Mathf.PI) / 3;
    const float c5 = (2 * Mathf.PI) / 4.5f;
    const float n1 = 7.5625f;
    const float d1 = 2.75f;

    public static float GetEasedValue(float x, Easing easingFunction)
    {
        float tmp;
        switch (easingFunction)
        {
            case Easing.Linear:
                return x; // This one is the correct implementation. The rest you have to replace from the functions from easings.net
            case Easing.InSine:
                return 1f - Mathf.Cos(x * Mathf.PI / 2f);
            case Easing.OutSine:
                return Mathf.Sin(x * Mathf.PI / 2f);
            case Easing.InOutSine:
                return -(Mathf.Cos(x * Mathf.PI) - 1f) / 2f;
            case Easing.InQuad:
                return x * x;
            case Easing.OutQuad:
                tmp = 1f - x;
                return 1f - tmp * tmp;
            case Easing.InOutQuad:
                tmp = (2f - 2f * x);
                return (x < 0.5f) ? 2f * x * x : 1f - tmp * tmp / 2;
            case Easing.InCubic:
                return x * x * x;
            case Easing.OutCubic:
                tmp = 1f - x;
                return tmp * tmp * tmp;
            case Easing.InOutCubic:
                tmp = 2f - 2f * x;
                return (x < 0.5f) ? 4f * x * x * x : 1f - tmp * tmp * tmp / 2f;
            case Easing.InQuart:
                return x * x * x * x;
            case Easing.OutQuart:
                tmp = 1f - x;
                tmp *= tmp;
                return 1f - tmp * tmp;
            case Easing.InOutQuart:
                tmp = 2f - 2f * x;
                tmp *= tmp;
                return (x < 0.5f) ? 8f * x * x * x * x : 1f - tmp * tmp / 2f;
            case Easing.InQuint:
                return x * x * x * x * x;
            case Easing.OutQuint:
                tmp = 1f - x;
                return tmp * tmp * tmp * tmp * tmp;
            case Easing.InOutQuint:
                tmp = 2f - 2f * x;
                return (x < 0.5) ? 16f * x * x * x * x * x : 1f - tmp * tmp * tmp * tmp * tmp / 2f;
            case Easing.InExpo:
                return (x == 0f) ? 0f : Mathf.Pow(2f, 10f * x - 10f);
            case Easing.OutExpo:
                return (x == 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * x);
            case Easing.InOutExpo:
                return (x == 0f)
                    ? 0f
                    : x == 1f
                    ? 1f
                    : x < 0.5f ? Mathf.Pow(2f, 20f * x - 10f) / 2f
                    : (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f;
            case Easing.InCirc:
                return 1 - Mathf.Sqrt(1 - x * x);
            case Easing.OutCirc:
                return Mathf.Sqrt(1f - (x - 1) * (x - 1));
            case Easing.InOutCirc:
                return (x < 0.5f)
                    ? (1 - Mathf.Sqrt(1f - 4f * x * x)) / 2f
                    : (Mathf.Sqrt(1f - (2f - 2f * x) * (2f - 2f * x)) + 1f) / 2f;
            case Easing.InBack:
                return c3 * x * x * x - c1 * x * x;
            case Easing.OutBack:
                return 1 + c3 * (x - 1f) * (x - 1f) * (x - 1f) + c1 * (x - 1f) * (x - 1f);
            case Easing.InOutBack:
                return (x < 0.5)
                    ? (4f * x * x * ((c2 + 1f) * 2 * x - c2)) / 2f
                    : ((2f * x - 2f) * (2f * x - 2f) * ((c2 + 1f) * (x * 2 - 2) + c2) + 2) / 2f;
            case Easing.InElastic:
                return (x == 0f)
                    ? 0
                    : x == 1
                    ? 1
                    : -Mathf.Pow(2f, 10f * x - 10f) * Mathf.Sin((x * 10f - 10.75f) * c4);
            case Easing.OutElastic:
                return (x == 0f)
                    ? 0f
                    : x == 1f
                    ? 1f
                    : Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1f;
            case Easing.InOutElastic:
                return (x == 0)
                    ? 0f
                    : x == 1f
                    ? 1f
                    : x < 0.5f
                    ? -(Mathf.Pow(2, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f
                    : (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) / 2f + 1f;
            case Easing.InBounce:
                return 1f - GetEasedValue(1f - x, Easing.OutBounce);
            case Easing.OutBounce:
                if (x < 1f / d1)
                {
                    return n1 * x * x;
                }
                else if (x < 2 / d1)
                {
                    return n1 * (x -= 1.5f / d1) * x + 0.75f;
                }
                else if (x < 2.5f / d1)
                {
                    return n1 * (x -= 2.25f / d1) * x + 0.9375f;
                }
                else
                {
                    return n1 * (x -= 2.625f / d1) * x + 0.984375f;
                }
            case Easing.InOutBounce:
                return (x < 0.5f)
                    ? (1 - GetEasedValue(1 - 2 * x, Easing.OutBounce)) / 2
                    : (1 + GetEasedValue(2 * x - 1, Easing.OutBounce)) / 2;
            default:
                return x;
        }
    }

    public enum Easing
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce,
    }


    /// <summary>
    /// Part of the Optional Tweening portion. Implement this function to automatically change a passed in value.
    /// To call this function, it will look like
    /// float x;
    /// StartCoroutine(Tween.ExecuteCoroutine((result) => x = result, startPosition.x, endPosition.x, time, Tween.Easing.Linear));
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="time"></param>
    /// <param name="easingFunction"></param>
    /// <returns></returns>
    public static IEnumerator ExecuteCoroutine(Action<float> callback, float start, float end, float time, Easing easingFunction)
    {
        // Your code here
        float t = 0f;

        while (t < 1f)
        {
            callback.Invoke(Mathf.LerpUnclamped(start, end, GetEasedValue(t, easingFunction)));

            t += Time.deltaTime / time;

            yield return null;
        }

        callback.Invoke(end);
    }

    public static IEnumerator TweenVector(Action<Vector3> callback, Vector3 start, Vector3 end, float time, Easing easingFunction)
    {
        float t = 0f;

        while (t < 1f)
        {
            callback.Invoke(Vector3.LerpUnclamped(start, end, GetEasedValue(t, easingFunction)));

            t += Time.deltaTime / time;

            yield return null;
        }

        callback.Invoke(end);
    }
}
