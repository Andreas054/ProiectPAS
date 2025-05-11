using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationCycle : MonoBehaviour
{
    public Sprite[] sprites; 
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    [SerializeField]
    private float timeBetweenSprites = 0.5f; 

    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length == 0)
        {
            Debug.LogError("Nu ai asignat sprite-urile!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenSprites)
        {
            timer = 0f;

            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }
}
