using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = .5f;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public int damage = 40;
    private bool bulletGoingHorizontal = true;

    private void Start()
    {
        rb.velocity = (bulletGoingHorizontal ? transform.right : transform.up) * speed;
        Destroy(gameObject, lifeTime);
    }
    public void SetDirection(bool goingHorizontal)
    {
        bulletGoingHorizontal = goingHorizontal;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        GameObject impactEffectInstance = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(impactEffectInstance, 0.5f);
    }
}