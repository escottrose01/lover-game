using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmileSpawner : MonoBehaviour
{
    public int numSmiles = 20;
    public float spawnTime = 1f;
    public int numRotations = 1;

    float distance = 1f;

    public void SpawnSmiles()
    {
        float totalRotation = 2f * Mathf.PI * numRotations;

        for (int i = 0; i < numSmiles; ++i)
        {
            float t = (float)i / numSmiles;
            float dx = distance * Mathf.Cos(totalRotation * t);
            float dy = distance * Mathf.Sin(totalRotation * t);
            Vector3 startPos = transform.position + new Vector3(dx, dy, 0);

            StartCoroutine(SpawnSmile(startPos, t * spawnTime));
        }
    }

    IEnumerator SpawnSmile(Vector3 startPos, float time)
    {
        yield return new WaitForSeconds(time);
        HUD.Instance.AnimateSmileCollect(startPos);
    }
}
