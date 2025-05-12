using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField]
    private GameObject key;
    [SerializeField]
    private GameObject winCanvas;

    void Start()
    {
        winCanvas.SetActive(false);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");
        if (other.CompareTag("Player") && key.GetComponent<KeyManager>().found)
        {
            Debug.Log("trigger2");
            winCanvas.SetActive(true);
        }
    }
}
