using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;

    private int health = 5;

    public int healingProgress = 0;
    private int healingCap = 5;

    public void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (healingProgress == healingCap)
        {
            healingProgress = 0;
            Heal(1);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        for (int i = 0; i < damage; i++)
        {
            healthBar.TakeDamage();
        }
    }

    public void Heal(int dmg)
    {
        health += dmg;
        for (int i = 0; i < dmg; i++)
        {
            healthBar.Heal();
        }
        if (health > 5)
        {
            health = 5;
        }
    }

    public void ResetHealth()
    {
        health = 5;
    }
}
