using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject StartScreenUI;
    public GameObject creditsMenuUI;

    public void Play()
    {
        SceneLoader.Instance.NextScene();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
