using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject tmp = new GameObject
                {
                    name = "TimeManager"
                };
                instance = tmp.AddComponent<TimeManager>();
            }

            return instance;
        }
    }

    private int freezeCounter;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void FreezeTime()
    {
        ++freezeCounter;
        Time.timeScale = 0f;
    }

    public void UnfreezeTime()
    {
        freezeCounter = Mathf.Max(0, freezeCounter - 1);
        if (freezeCounter == 0) Time.timeScale = 1f;
    }

    public void HardUnfreezeTime()
    {
        freezeCounter = 0;
        Time.timeScale = 1f;
    }
}
