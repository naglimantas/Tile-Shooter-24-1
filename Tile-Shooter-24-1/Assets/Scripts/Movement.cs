using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public LayerMask groundLayer;
    public float groundDistance;
    public float jumpSpeed;
    public float moveSpeed;
    public ParticleSystem dust;
    private bool doubleJump;
    public bool grounded;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);
        grounded = hit.collider != null;  
        
        if(grounded && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        } 

        if(Input.GetButtonDown("Jump"))
        {
            if(grounded || doubleJump)
            {
                Jump();
                doubleJump = !doubleJump;
                CreateDust();
            }

        }
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
        if(h != 0) 
        {
            float angle = h > 0 ? 0 : 180;
            transform.eulerAngles = new Vector3(0, angle, 0);
        }

    }

    void Jump()
    {
        rb.velocity = Vector2.up * jumpSpeed;
    }
    void CreateDust()
    {
        dust.Play();
    }
}
