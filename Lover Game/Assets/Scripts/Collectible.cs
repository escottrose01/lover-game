using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    public UnityEvent onCollect;
    public CollectibleType collectibleType = CollectibleType.Other;
    public AudioClip collectClip;

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
                    AudioManager.Instance.PlayGoalCollect();
                    break;
                default:
                    AudioManager.Instance.PlaySound(collectClip, 0.25f);
                    break;
            }
        }
    }

    public enum CollectibleType { Heart, Smile, Goal, Other }
}