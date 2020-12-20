using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Fireable
{
    public Sprite happyBulletSprite;

    GameObject bulletPrefab;

    private void Awake()
    {
        // create bullet prefab
        bulletPrefab = new GameObject();
        bulletPrefab.name = "Bullet";
        bulletPrefab.SetActive(false);
        Projectile projectile = bulletPrefab.AddComponent<Projectile>();
        projectile.isEnemyProjectile = false;
        SpriteRenderer sr = bulletPrefab.AddComponent<SpriteRenderer>();
        sr.sprite = happyBulletSprite;
        bulletPrefab.AddComponent<CircleCollider2D>();
        Rigidbody2D rb = bulletPrefab.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.useFullKinematicContacts = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Fire(Vector2 direction)
    {
        if (cooldown.CheckReady())
        {
            cooldown.StartCooldown();
            GameObject obj = Instantiate(bulletPrefab, transform.position + Vector3.forward, Quaternion.identity);
            obj.SetActive(true);
            obj.GetComponent<Projectile>().FireProjectile(direction);
        }
    }
}
