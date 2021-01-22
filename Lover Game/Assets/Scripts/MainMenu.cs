using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject startScreenUI;
    public GameObject optionsMenuUI;
    public GameObject levelSelectUI;
    public GameObject panelHolder;

    public Slider musicSlider;
    public Slider soundSlider;

    RectTransform startScreenRect;
    RectTransform optionsMenuRect;
    RectTransform levelSelectRect;
    RectTransform panelHolderRect;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.MusicLevelMax;
        soundSlider.value = AudioManager.Instance.SoundLevelMax;

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

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.MusicLevelMax = value;
    }

    public void SetSoundVolume(float value)
    {
        AudioManager.Instance.SoundLevelMax = value;
    }
}
