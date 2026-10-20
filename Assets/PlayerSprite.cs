using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    private Rigidbody2D rb;
    private float xMove;

    [Header("Inscribed")]
    public float moveSpeed = 6f; // Movement speed of player
    public float edge = 8.2f; // Map boundaries

    public GameObject projPrefab;
    public Transform projSpawnLoc; // Used to make sure projectile spawns from ship

    [Header("Shooting Settings")]
    public float fireRate = 0.5f; // Fire rate of player
    private float fireTime = 0f; // Used to create delay between attacks
    public float projSpeed = 7f; // Speed of projectiles

    void Start() {
        rb = GetComponent<Rigidbody2D>(); // Get 2D Rigidbody first
    }

    // Movement input using A and D or Left and Right Arrows
    void Update() {
        xMove = Input.GetAxisRaw("Horizontal"); // Take in A, D, <-, -> inputs

        // Space bar/Click for attacks
        if (Time.time >= fireTime) { // Make sure attacks are delayed
            if (Input.GetKeyDown(KeyCode.Space)) { // Spacebar attack
                spacebarAttack(); 
            }
            else if (Input.GetMouseButtonDown(0)) { // Left click attack
                clickAttack();
            }
        }
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

    void playerSpawnProj() {
        // Spawn projectiles
        fireTime = Time.time + fireRate; // Calculate attack delay
        GameObject attack = Instantiate(projPrefab, projSpawnLoc.position, Quaternion.identity);
        attack.layer = LayerMask.NameToLayer("PlayerProj"); // Ensure projectile is linked to player
        
        // Move projectiles up
        Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.linearVelocity = Vector2.up * projSpeed;
        }

        Destroy(attack, 1.5f); // Delete attack object after certain time
    }

    void spacebarAttack() {
        // Space bar projectiles
        playerSpawnProj();
        
    }

    void clickAttack() {
        // Click projectiles
        fireTime = Time.time + fireRate; // Calculate attack delay
        playerSpawnProj();
    }


}


