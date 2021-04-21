using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isEnemyProjectile;
    public float bulletSpeed = 20f;
    public float bulletDistance = 20f;

    float travelPercentage;
    bool fired;
    Vector3 targetPosition;
    Vector3 startPosition;

    private void Update()
    {
        if (fired)
        {
            travelPercentage += Time.deltaTime * bulletSpeed / bulletDistance;
            transform.position = Vector3.Lerp(startPosition, targetPosition, Tween.GetEasedValue(travelPercentage, Tween.Easing.OutQuad));

            if (travelPercentage >= 1f) Destroy(gameObject); 
        }
    }

    public void FireProjectile(Vector3 direction)
    {
        travelPercentage = 0f;
        fired = true;
        startPosition = transform.position;
        targetPosition = startPosition + bulletDistance * direction.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) {
            Hittable hittable = collision.gameObject.GetComponent<Hittable>();
            if (hittable != null)
            {
                hittable.onHit.Invoke();
            }
            Destroy(gameObject);
        }
    }
}
