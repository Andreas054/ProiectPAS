using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health = 5;
    private bool inRange = false;
    private float damageRange = 1.0f;

    private float damageCooldown = 1f;
    private float lastDamageTime = -999f;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerHealth>().healingProgress += 1;
            GameObject CoinQuantity = GameObject.Find("CoinQuantity");

            int currency = int.Parse(CoinQuantity.GetComponent<TextMeshProUGUI>().text);
            currency += 1;
            CoinQuantity.GetComponent<TextMeshProUGUI>().text = currency.ToString();

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - lastDamageTime >= damageCooldown)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(1);
            lastDamageTime = Time.time;
        }
    }


    public void TakeDamage(int damage, Vector3 attackerPosition)
    {
        health -= damage;

        // knockback
        Vector2 knockbackDir = (transform.position - (Vector3)attackerPosition).normalized;
        float knockbackSpeed = 5f;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = knockbackDir * knockbackSpeed;

        StartCoroutine(ResetVelocity());
    }

    private IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
