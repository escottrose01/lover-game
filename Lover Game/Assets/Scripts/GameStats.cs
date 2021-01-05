using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    private static GameStats instance;
    public static GameStats Instance { get { return instance; } }

    public static readonly int numLevels = 3; // not including secret level
    public static int CurLevel { get; private set; }
    public static bool SecretLevelUnlocked { get; private set; }

    static int[] smileCount;
    static int[] heartCount;

    int levelHeartCount;
    int levelSmileCount;
    int levelNum;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            smileCount = new int[numLevels];
            heartCount = new int[numLevels];

            ReadStatsFromSave();
        }
    }

    public void IncrementHearts()
    {
        ++levelHeartCount;
    }

    public void IncrementSmiles()
    {
        ++levelSmileCount;
    }

    public void SaveCounts()
    {
        heartCount[levelNum] = Mathf.Max(heartCount[levelNum], levelHeartCount);
        smileCount[levelNum] = Mathf.Max(smileCount[levelNum], levelSmileCount);
    }

    void ReadStatsFromSave()
    {
        if (PlayerPrefs.HasKey("SaveFlag"))
        {
            CurLevel = PlayerPrefs.GetInt("CurLevel");

            for (int i = 0; i < numLevels; ++i)
            {
                heartCount[i] = PlayerPrefs.GetInt("Hearts_Level" + i.ToString());
                smileCount[i] = PlayerPrefs.GetInt("Smiles_Level" + i.ToString());
            }
        }
    }

    public void SaveStats()
    {
        PlayerPrefs.SetString("SaveFlag", "Hi Khrea! I love you lots <3");
        PlayerPrefs.SetInt("CurLevel", CurLevel);

        for (int i = 0; i < numLevels; ++i)
        {
            PlayerPrefs.SetInt("Hearts_Level" + i.ToString(), heartCount[i]);
            PlayerPrefs.SetInt("Smiles_Level" + i.ToString(), smileCount[i]);
        }
    }

    public void ClearStats()
    {
        PlayerPrefs.DeleteAll();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SaveCounts();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        levelHeartCount = 0;
        levelSmileCount = 0;
        levelNum = SceneManager.GetActiveScene().buildIndex - 1;

        SaveStats();
    }
}
