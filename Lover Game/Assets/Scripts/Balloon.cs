using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public GameObject playerBalloonPrefab;

    public void SpawnPlayerBalloon()
    {
        GameObject obj = Instantiate(playerBalloonPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<PlayerBalloon>().AttachToPlayer();
    }
}
