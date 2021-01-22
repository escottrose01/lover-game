using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMenu : MonoBehaviour
{
    public GameObject levelArea;
    public GameObject levelIcon;
    public Sprite LevelClearSprite;
    public Sprite LevelUnlockedSprite;
    public Sprite LevelLockedSprite;
    public int numCols;

    private IEnumerator Start()
    {
        // I don't know why I have to do this terribleness. Stupud WebGL!
        yield return new WaitForSecondsRealtime(0.1f);

        int numRows = GameStats.numLevels / numCols;
        if (GameStats.numLevels % numRows != 0) ++numCols;

        VerticalLayoutGroup verticalLayout = levelArea.AddComponent<VerticalLayoutGroup>();
        verticalLayout.childAlignment = TextAnchor.MiddleCenter;

        //verticalLayout.childControlHeight = false;
        //verticalLayout.childControlWidth = false;

        for (int r = 0; r < numRows; ++r)
        {
            GameObject curRow = new GameObject
            {
                name = "Row" + (r + 1).ToString()
            };
            curRow.AddComponent<RectTransform>();
            curRow.transform.SetParent(levelArea.transform);
            curRow.transform.localScale = Vector3.one;

            HorizontalLayoutGroup horizontalLayout = curRow.AddComponent<HorizontalLayoutGroup>();
            horizontalLayout.childControlWidth = false;
            horizontalLayout.childControlHeight = false;
            horizontalLayout.childAlignment = TextAnchor.MiddleCenter;

            for (int c = 0; c < numCols; ++c)
            {
                int levelNum = r * numCols + c + 1;
                if (levelNum <= GameStats.numLevels)
                {
                    GameObject obj = Instantiate(levelIcon, curRow.transform);
                    obj.name = "LevelButton-" + levelNum.ToString();
                    obj.GetComponentInChildren<TMP_Text>().text = "Level " + levelNum.ToString();

                    //RectTransform rectTransform = obj.GetComponentInChildren<RectTransform>();
                    //Rect rect = rectTransform.rect;
                    //rect.width = 100;
                    //rect.height = 100;
                    //rectTransform.sizeDelta = 100 * Vector2.one;

                    Button button = obj.GetComponent<Button>();
                    Image image = obj.GetComponent<Image>();

                    button.onClick.AddListener(() => SceneLoader.Instance.LoadScene(levelNum));

                    if (levelNum < GameStats.CurLevel) image.sprite = LevelClearSprite;
                    else if (levelNum == GameStats.CurLevel)
                    {
                        image.sprite = LevelUnlockedSprite;

                        ColorBlock colors = button.colors;
                        colors.normalColor = 0.8f * Color.white;
                        button.colors = colors;
                    }
                    else
                    {
                        image.sprite = LevelLockedSprite;
                        button.interactable = false;
                    }
                }
            }
        }

        if (GameStats.SecretLevelUnlocked)
        {
            GameObject secretRow = new GameObject
            {
                name = "Row" + (numRows + 1).ToString()
            };
            secretRow.AddComponent<RectTransform>();
            secretRow.transform.SetParent(levelArea.transform);

            HorizontalLayoutGroup secretHorizontal = secretRow.AddComponent<HorizontalLayoutGroup>();
            secretHorizontal.childControlWidth = false;
            secretHorizontal.childControlHeight = false;
            secretHorizontal.childAlignment = TextAnchor.MiddleCenter;

            GameObject button = Instantiate(levelIcon, secretRow.transform);
            button.name = "LevelButton-" + (GameStats.numLevels + 1).ToString();
            button.GetComponentInChildren<TMP_Text>().text = "Level " + (GameStats.numLevels + 1).ToString();
            button.GetComponent<Button>().onClick.AddListener(() => SceneLoader.Instance.LoadScene(GameStats.numLevels + 1));
        }
    }
}
