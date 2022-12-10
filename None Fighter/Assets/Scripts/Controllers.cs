using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controllers : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float hitRange = 0.5f;

    GameObject enemy; 

    public LayerMask enemyLayer;
    public Transform hitDistence;

    Animator animator;
    CapsuleCollider2D collider;
    LayerMask layer;
    Vector2 moveInput;
    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        layer = LayerMask.GetMask("Ground");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        if(moveInput.x != 0)
            transform.localScale = new Vector3(moveInput.x, transform.localScale.y, transform.localScale.z);

        if (collider.IsTouchingLayers(layer))
            animator.SetBool("isJumping", false);
        else 
            animator.SetBool("isJumping", true);
    }

    void OnJump(InputValue value)
    {
        Debug.Log("We Jumped!");

        if (value.isPressed && collider.IsTouchingLayers(layer))
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitDistence.position, hitRange, enemyLayer);
        animator.SetBool("isAttacking", true);
        Debug.Log(animator.GetBool("isAttacking"));

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We Hit " + enemy + '!');
            Destroy(enemy.gameObject);
        }

        Invoke("animinvoke", 0.1f);
    }

    void animinvoke()
    {
        animator.SetBool("isAttacking", false);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        if (playerVelocity.x != 0)
            animator.SetBool("isRunning", true);

        else
            animator.SetBool("isRunning", false);
    }
}
