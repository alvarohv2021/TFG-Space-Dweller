using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public GameObject deathEffect;
    public playerMovement player;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<playerMovement>();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            if (player != null && player.interfaces != null)
            {
                player.interfaces.ScorePoints(maxHealth);
            }
            else
            {
                Debug.LogError("player o player.interfaces no está asignado");
            }
        }
    }

    private void Die()
    {
        deathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(deathEffect, .4f);
    }
}
