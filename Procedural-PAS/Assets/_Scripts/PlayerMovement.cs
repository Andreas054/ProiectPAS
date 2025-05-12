using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody2D rb;
    [SerializeField] private int speed = 5;
    private Animator animator;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        //Flip();
    }

    private void Flip()
    {
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    //private void FixedUpdate()
    //{
    //    if (movement.x != 0 || movement.y != 0)
    //    {
    //        rb.velocity = movement * speed;
    //    }
    //}

    private void FixedUpdate()
    {
        if (movement.x != 0 || movement.y != 0)
        {
            rb.velocity = movement * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void SetCanMove(bool state)
    {
        canMove = state;
    }
}
