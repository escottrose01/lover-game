using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    private static HUD instance;
    public static HUD Instance { get { return instance; } }

    public TMP_Text heartText;
    public TMP_Text smileText;
    public Sprite heartSprite;
    public Sprite smileSprite;

    RectTransform parentRect;
    RectTransform smileRect;
    RectTransform heartRect;

    float spriteScreenWidth;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        parentRect = heartText.transform.parent.GetComponent<RectTransform>();
        smileRect = smileText.transform.parent.GetComponent<RectTransform>();
        heartRect = heartText.transform.parent.GetComponent<RectTransform>();

        spriteScreenWidth = (Camera.main.WorldToScreenPoint(heartSprite.bounds.max) - Camera.main.WorldToScreenPoint(heartSprite.bounds.min)).x;
    }

    public void UpdateText()
    {
        heartText.text = "x " + GameStats.Instance.LevelHeartCount;
        smileText.text = "x " + GameStats.Instance.LevelSmileCount;
    }

    public void AnimateHeartCollect(Vector3 startPos)
    {
        GameObject heart = new GameObject("Heart");
        heart.transform.parent = heartText.transform.parent;
        heart.AddComponent<Image>().sprite = heartSprite;
        RectTransform rt = heart.GetComponent<RectTransform>();

        Vector2 startScreenPos = Camera.main.WorldToScreenPoint(startPos);
        rt.position = startScreenPos;
        rt.sizeDelta = spriteScreenWidth * heart.transform.localScale * Vector2.one;
        heart.transform.localScale = Vector3.one;

        StartCoroutine(Tween.TweenVector(x => rt.anchoredPosition = x, rt.anchoredPosition, Vector3.zero, 1f, Tween.Easing.InOutCubic));
        StartCoroutine(Tween.TweenVector(x => rt.sizeDelta = x, rt.sizeDelta, smileRect.sizeDelta, 1f, Tween.Easing.Linear));
        Destroy(heart, 1.1f);
        Invoke(nameof(CollectHeart), 1.1f);

        AudioManager.Instance.PlayItemPickup();
    }

    public void AnimateSmileCollect(Vector3 startPos)
    {
        GameObject smile = new GameObject("Smile");
        smile.transform.parent = smileText.transform.parent;
        smile.AddComponent<Image>().sprite = smileSprite;
        RectTransform rt = smile.GetComponent<RectTransform>();

        Vector2 startScreenPos = Camera.main.WorldToScreenPoint(startPos);
        rt.position = startScreenPos;
        rt.sizeDelta = spriteScreenWidth * smile.transform.localScale * Vector2.one;
        smile.transform.localScale = Vector3.one;

        StartCoroutine(Tween.TweenVector(x => rt.anchoredPosition = x, rt.anchoredPosition, Vector3.zero, 1f, Tween.Easing.InOutCubic));
        StartCoroutine(Tween.TweenVector(x => rt.sizeDelta = x, rt.sizeDelta, smileRect.sizeDelta, 1f, Tween.Easing.Linear));
        Destroy(smile, 1.1f);
        Invoke(nameof(CollectSmile), 1.1f);

        AudioManager.Instance.PlayItemPickup();
    }

    void CollectSmile()
    {
        GameStats.Instance.IncrementSmiles();
        UpdateText();
    }

    void CollectHeart()
    {
        GameStats.Instance.IncrementHearts();
        UpdateText();
    }
}
