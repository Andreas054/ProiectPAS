using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] public int chestType; // 1 2
    public float health; // 1 2

    void Start()
    {
        health = chestType;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject CoinQuantity = GameObject.Find("CoinQuantity");

            int currency = int.Parse(CoinQuantity.GetComponent<TextMeshProUGUI>().text);
            currency += 5 * chestType;
            CoinQuantity.GetComponent<TextMeshProUGUI>().text = currency.ToString();
        }
    }
}
