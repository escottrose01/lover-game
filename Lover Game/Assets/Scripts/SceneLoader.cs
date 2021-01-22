using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Animator transition;

    private float transitionTime = 2f;

    private IEnumerator transitionRoutine;

    private static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject tmp = new GameObject
                {
                    name = "SceneLoader"
                };
                instance = tmp.AddComponent<SceneLoader>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        transition = GetComponent<Animator>();
    }

    public void KillVolume()
    {
        StartCoroutine(Tween.ExecuteCoroutine(x => AudioManager.Instance.MusicLevelPercent = x, 1f, 0f, transitionTime, Tween.Easing.Linear));
    }

    public void RaiseVolume()
    {
        StartCoroutine(Tween.ExecuteCoroutine(x => AudioManager.Instance.MusicLevelPercent = x, 0f, 1f, transitionTime, Tween.Easing.Linear));
    }

    public void Restart()
    {
        if (transitionRoutine == null)
        {
            transitionRoutine = NextSceneCoroutine(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(transitionRoutine);
        }
    }

    public void MainMenu()
    {
        if (transitionRoutine == null)
        {
            transitionRoutine = NextSceneCoroutine(0);
            StartCoroutine(transitionRoutine);
        }
    }

    public void NextScene()
    {
        if (transitionRoutine == null)
        {
            transitionRoutine = NextSceneCoroutine((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            StartCoroutine(transitionRoutine);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (transitionRoutine == null)
        {
            transitionRoutine = NextSceneCoroutine(sceneIndex);
            StartCoroutine(transitionRoutine);
        }
    }

    private IEnumerator NextSceneCoroutine(int buildIndex)
    {
        // fallback transition
        if (transition == null)
        {
            yield return new WaitForSecondsRealtime(transitionTime);
            SceneManager.LoadScene(buildIndex);
            yield break;
        }

        // start transition-in animation
        transition.SetTrigger("Start");

        // wait
        yield return new WaitForSecondsRealtime(transitionTime);

        // change scene
        SceneManager.LoadScene(buildIndex);

        // start transition-out animation
        transition.SetTrigger("End");

        transitionRoutine = null;
    }
}
