using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    public GameObject pauseMenuUI;
    public Slider musicSlider;
    public Slider soundSlider;

    public bool Paused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.MusicLevelMax;
        soundSlider.value = AudioManager.Instance.SoundLevelMax;
    }

    public void TogglePause()
    {
        if (Paused) Unpause();
        else Pause();
    }

    void Unpause()
    {
        Paused = false;
        TimeManager.Instance.UnfreezeTime();
        pauseMenuUI.SetActive(false);
    }

    void Pause()
    {
        Paused = true;
        TimeManager.Instance.FreezeTime();
        pauseMenuUI.SetActive(true);
    }

    public void Quit()
    {
        SceneLoader.Instance.MainMenu();
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
