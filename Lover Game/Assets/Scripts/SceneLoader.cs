using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private float fadeTime = 0.5f;

    private IEnumerator fadeRoutine;

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

    public void Restart()
    {
        if (fadeRoutine == null)
        {
            fadeRoutine = NextSceneCoroutine(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(fadeRoutine);
        }
    }

    public void MainMenu()
    {
        if (fadeRoutine == null)
        {
            fadeRoutine = NextSceneCoroutine(0);
            StartCoroutine(fadeRoutine);
        }
    }

    public void NextScene()
    {
        if (fadeRoutine == null)
        {
            fadeRoutine = NextSceneCoroutine((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            StartCoroutine(fadeRoutine);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        if (fadeRoutine == null)
        {
            fadeRoutine = NextSceneCoroutine(sceneIndex);
            StartCoroutine(fadeRoutine);
        }
    }

    private IEnumerator NextSceneCoroutine(int buildIndex)
    {
        if (canvasGroup == null)
        {
            SceneManager.LoadScene(buildIndex);
            yield break;
        }

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }

        SceneManager.LoadScene(buildIndex);
        yield return new WaitForSeconds(0.25f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }

        fadeRoutine = null;
    }
}
