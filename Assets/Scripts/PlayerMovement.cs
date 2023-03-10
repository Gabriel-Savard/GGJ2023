using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public SpriteRenderer renderer;
    public Animator animator;
    public float moveSpeed;
    private Vector2 movement;
    public Transform attackPoint;

    public float idleTimer = 0;

    // Dash variables
    private float activeMoveSpeed;
    public float dashSpeed;

    public float dashLength = .5f, dashCooldown = 1f;

    private float dashCounter;
    private float dashCoolCounter;

    void Start()
    {
        activeMoveSpeed = moveSpeed;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x < 0)
        {
            renderer.flipX = true;
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);
        }
        else if (movement.x > 0)
        {
            renderer.flipX = false;
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);
        }

        if (movement.sqrMagnitude > 0)
            idleTimer = 0;
        else
            idleTimer += Time.deltaTime;

        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("IdleTime", idleTimer);

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                animator.SetBool("Dash", true);
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
                animator.SetBool("Dash", false);
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + movement * activeMoveSpeed * Time.fixedDeltaTime);
    }
}