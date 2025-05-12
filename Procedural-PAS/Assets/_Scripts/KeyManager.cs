using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class KeyManager : MonoBehaviour
{
    public bool found;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ResetKey()
    {
        found = false;
        Color newColor = gameObject.GetComponent<SpriteRenderer>().color;
        newColor.a = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Destroy(gameObject);
            found = true;
            Color newColor = gameObject.GetComponent<SpriteRenderer>().color;
            newColor.a = 0f;
            gameObject.GetComponent<SpriteRenderer>().color = newColor;
        }
    }
}
