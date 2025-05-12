using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject keyIcon;
    [SerializeField]
    private GameObject key;
    private bool flagKeyActive;

    void Awake()
    {
        ResetPlayer();
    }

    void Update()
    {
        if (!flagKeyActive && key.GetComponent<KeyManager>().found)
        {
            keyIcon.SetActive(true);
            flagKeyActive = true;
        }
    }

    public void ResetPlayer()
    {
        //Debug.Log("fdyd");
        keyIcon.SetActive(false);
        flagKeyActive = false;

        gameObject.GetComponent<PlayerHealth>().ResetHealth();

        GameObject CoinQuantity = GameObject.Find("CoinQuantity");
        CoinQuantity.GetComponent<TextMeshProUGUI>().text = "0";

        key.GetComponent<KeyManager>().ResetKey();
    }
}
