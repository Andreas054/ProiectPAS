using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject heartsContainer;
    [SerializeField]
    private GameObject heartPrefab;

    private void Awake()
    {
        foreach (Transform child in heartsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject instance = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity);
            instance.transform.SetParent(heartsContainer.transform);
        }
    }
    public void TakeDamage()
    {
        if (heartsContainer.transform.childCount > 0)
        {
            Transform lastChild = heartsContainer.transform.GetChild(transform.childCount - 1);
            Destroy(lastChild.gameObject);
        }
    }

    public void Heal()
    {
        if (heartsContainer.transform.childCount == 5)
            return;

        GameObject instance = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity);
        instance.transform.SetParent(heartsContainer.transform);
    }
}
