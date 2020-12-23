using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public int numLevels;
    public GameObject startScreenUI;
    public GameObject optionsMenuUI;
    public GameObject levelSelectUI;
    public GameObject panelHolder;

    RectTransform startScreenRect;
    RectTransform optionsMenuRect;
    RectTransform levelSelectRect;
    RectTransform panelHolderRect;

    private void Start()
    {
        startScreenRect = startScreenUI.GetComponent<RectTransform>();
        optionsMenuRect = optionsMenuUI.GetComponent<RectTransform>();
        levelSelectRect = levelSelectUI.GetComponent<RectTransform>();
        panelHolderRect = panelHolder.GetComponent<RectTransform>();
    }

    public void ShowOptions()
    {
        optionsMenuRect.localPosition = panelHolderRect.localPosition + panelHolderRect.rect.width * Vector3.right;
        levelSelectRect.localPosition = panelHolderRect.localPosition + panelHolderRect.rect.height * Vector3.down;
        StartCoroutine(Tween.TweenVector(x => panelHolderRect.localPosition = x, panelHolderRect.localPosition, panelHolderRect.localPosition + panelHolderRect.rect.width * Vector3.left, 0.5f, Tween.Easing.InOutQuad));
    }

    public void ShowLevels()
    {
        levelSelectRect.localPosition = panelHolderRect.localPosition + panelHolderRect.rect.width * Vector3.right;
        optionsMenuRect.localPosition = panelHolderRect.localPosition + panelHolderRect.rect.height * Vector3.down;
        StartCoroutine(Tween.TweenVector(x => panelHolderRect.localPosition = x, panelHolderRect.localPosition, panelHolderRect.localPosition + panelHolderRect.rect.width * Vector3.left, 0.5f, Tween.Easing.InOutQuad));
    }

    public void Back()
    {
        StartCoroutine(Tween.TweenVector(x => panelHolderRect.localPosition = x, panelHolderRect.localPosition, panelHolderRect.localPosition + panelHolderRect.rect.width * Vector3.right, 0.5f, Tween.Easing.InOutQuad));
    }

    public void Play()
    {
        SceneLoader.Instance.NextScene();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
