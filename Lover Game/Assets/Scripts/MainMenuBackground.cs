using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackground : MonoBehaviour
{
    public Sprite heartSprite;
    public float fallSpeed = 100f;
    public float rotateSpeed = 50f;

    int numHearts = 100;
    float minY;
    float maxY;
    GameObject[] hearts;
    float[] scales;
    float[] rotateScales;

    // Start is called before the first frame update
    void Start()
    {
        hearts = new GameObject[numHearts];
        scales = new float[numHearts];
        rotateScales = new float[numHearts];

        Rect bounds = GetComponent<RectTransform>().rect;
        minY = bounds.min.y - 2 * Mathf.Sqrt(2) * heartSprite.rect.height;
        maxY = bounds.max.y + 2 * Mathf.Sqrt(2) * heartSprite.rect.height;

        float height = maxY - minY;
        float width = bounds.width;

        for (int i = 0; i < numHearts; ++i)
        {
            scales[i] = Random.Range(0.5f, 1f);
            rotateScales[i] = Random.Range(-2f, 2f);
            hearts[i] = new GameObject
            {
                name = "Heart"
            };
            hearts[i].transform.SetParent(transform);
            hearts[i].transform.localScale = scales[i] * Vector3.one;
            hearts[i].AddComponent<Image>().sprite = heartSprite;

            float x = Random.Range(-width / 2, width / 2);
            float y = Random.Range(-height / 2, height / 2);
            hearts[i].transform.localPosition = new Vector3(x, y, 0f);
        }
    }

    private void Update()
    {
        for (int i = 0; i < numHearts; ++i)
        {
            Vector3 position = hearts[i].transform.localPosition;
            position.y -= Time.deltaTime * fallSpeed * scales[i];
            if (position.y < minY) position.y = maxY;

            hearts[i].transform.localPosition = position;
            hearts[i].transform.Rotate(Vector3.forward, rotateSpeed * rotateScales[i] * Time.deltaTime);
        }
    }
}
