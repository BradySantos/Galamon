using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    private Rigidbody2D rb;
    private float xMove;

    [Header("Inscribed")]
    public float moveSpeed = 5f;
    public float edge = 8.2f;

    void Start() {
        rb = GetComponent<Rigidbody2D>(); // Get 2D Rigidbody first
    }

    // Movement input using A and D or Left and Right Arrows
    void Update() {
        xMove = Input.GetAxisRaw("Horizontal"); // Take in A, D, <-, -> inputs
    }

    void FixedUpdate() {
        rb.linearVelocity =  new Vector2(xMove * moveSpeed, rb.linearVelocity.y); // Make movement smooth
        
        // Set boundaries at edges
        Vector2 pos = rb.position; // Current position

        if ((pos.x < -edge) && (xMove < 0)) { // Left bounds
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if ((pos.x > edge) && (xMove > 0)) { // Right bounds
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}


