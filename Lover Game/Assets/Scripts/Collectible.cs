using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    public UnityEvent onCollect;
    public CollectibleType collectibleType = CollectibleType.Other;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onCollect.Invoke();
            Destroy(transform.gameObject);

            switch (collectibleType)
            {
                case CollectibleType.Heart:
                    HUD.Instance.AnimateHeartCollect(transform.position);
                    break;
                case CollectibleType.Smile:
                    HUD.Instance.AnimateSmileCollect(transform.position);
                    break;
                case CollectibleType.Goal:
                    SceneLoader.Instance.NextScene();
                    break;
                default:
                    break;
            }
        }
    }

    public enum CollectibleType { Heart, Smile, Goal, Other }
}