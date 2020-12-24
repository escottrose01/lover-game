using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMenu : MonoBehaviour
{
    public GameObject levelArea;
    public GameObject levelIcon;
    public int numLevels;
    public int numCols;

    Rect panelDimensions;
    Rect iconDimensions;

    private void Start()
    {
        panelDimensions = GetComponent<RectTransform>().rect;
        iconDimensions = levelIcon.GetComponent<RectTransform>().rect;

        int numRows = numLevels / numCols;
        if (numLevels % numRows != 0) ++numCols;

        VerticalLayoutGroup verticalLayout = levelArea.AddComponent<VerticalLayoutGroup>();
        verticalLayout.childAlignment = TextAnchor.MiddleCenter;

        for (int r = 0; r < numRows; ++r)
        {
            GameObject curRow = new GameObject
            {
                name = "Row" + (r + 1).ToString()
            };
            curRow.AddComponent<RectTransform>();
            curRow.transform.SetParent(levelArea.transform);

            HorizontalLayoutGroup horizontalLayout = curRow.AddComponent<HorizontalLayoutGroup>();
            horizontalLayout.childControlWidth = false;
            horizontalLayout.childControlHeight = false;
            horizontalLayout.childAlignment = TextAnchor.MiddleCenter;

            for (int c = 0; c < numCols; ++c)
            {
                int levelNum = r * numCols + c + 1;
                if (levelNum <= numLevels)
                {
                    GameObject button = Instantiate(levelIcon, curRow.transform);
                    button.name = "LevelButton-" + levelNum.ToString();
                    button.GetComponentInChildren<TMP_Text>().text = "Level " + levelNum.ToString();
                    button.GetComponent<Button>().onClick.AddListener(() => SceneLoader.Instance.LoadScene(levelNum));
                }
            }
        }

        bool superSecretBool = false;
        if (superSecretBool)
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
            button.name = "LevelButton-" + (numLevels + 1).ToString();
            button.GetComponentInChildren<TMP_Text>().text = "Level " + (numLevels + 1).ToString();
            button.GetComponent<Button>().onClick.AddListener(() => SceneLoader.Instance.LoadScene(numLevels + 1));
        }
    }
}
