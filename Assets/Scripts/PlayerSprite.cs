using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSprite : MonoBehaviour
{
    // Animation
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Sprite thunderboltSprite;

    // Movement
    private Rigidbody2D rb;
    private float xMove;

    // Projectile
    public GameObject projPrefab;
    public Transform projSpawnLoc; // Used to make sure projectile spawns from ship

    // Health
    public Transform healthBar;
    public float maxHealth;
    private Vector3 originalScale;


    [Header("Inscribed")]
    public float moveSpeed = 6f; // Movement speed of player
    public float edge = 8.2f; // Map boundaries

    [Header("Shooting Settings")]
    public float fireRate = 0.5f; // Fire rate of player
    private float fireTime = 0f; // Used to create delay between attacks
    public float projSpeed = 7f; // Speed of projectiles

    [Header("Player Stats")]
    public float health = 10f;
    public float dmg = 1f;

    [Header("Audio")] 
    public AudioClip deathSound;
    public AudioClip getHitSound;
    public AudioSource getHitAudioSource;

    void Start() {
        animator = GetComponent<Animator>(); // Get Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get Sprite Renderer component
        rb = GetComponent<Rigidbody2D>(); // Get 2D Rigidbody 

        maxHealth = health;
        originalScale = healthBar.localScale;
    }

    void Update() { // Movement input using A and D or Left and Right Arrows
        xMove = Input.GetAxisRaw("Horizontal"); // Take in A, D, <-, -> inputs

        // Sprite animation - whether player is moving
        animator.SetBool("IsWalking", xMove != 0);

        // Flip walking direction
        if (xMove > 0) { // Face right
            spriteRenderer.flipX = true;
        }
        else if (xMove < 0) { // Face left
            spriteRenderer.flipX = false;
        }

        // Space bar/Click for attacks
        if (Time.time >= fireTime) { // Make sure attacks are delayed
            if (Input.GetKeyDown(KeyCode.Space)) { // Spacebar attack
                SpacebarAttack(); 
            }
            else if (Input.GetMouseButtonDown(0)) { // Left click attack. Skip if clicking UI button
                if ((EventSystem.current != null) && (EventSystem.current.IsPointerOverGameObject())) {
                    return;
                }
                else {
                    ClickAttack();
                }
            }
        }
    }

    void FixedUpdate() { // Movement Boundaries
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
    
    public void TakeDamage(float damage) { // Player takes damage
        health -= damage;
        health = Mathf.Max(health, 0); // Make sure health does not go below 0

        float healthPercent = health / maxHealth;

        healthBar.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, 
        originalScale.z);

        if ((health > 0) && (!getHitAudioSource.isPlaying)) { // Only play damage sound if not taking fatal hit; no overlap
            getHitAudioSource.clip = getHitSound;
            getHitAudioSource.volume = 0.7f;
            getHitAudioSource.time = 8.5f;
            getHitAudioSource.Play();
        }

        if (health <= 0) { // Death and game over screen
            AudioManager.instance.PlaySound(deathSound, 1f);
            Destroy(gameObject);
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            gameManager.GameOver();
        }
    }

    void PlayerSpawnProj() {
        // Spawn projectiles
        fireTime = Time.time + fireRate; // Calculate attack delay
        
        GameObject attack = Instantiate(projPrefab, projSpawnLoc.position, Quaternion.identity);
        attack.GetComponent<PlayerProjectile>().dmg = dmg; // Give projectile dmg value of player
        attack.layer = LayerMask.NameToLayer("PlayerProj"); // Ensure projectile is linked to player
        attack.GetComponent<SpriteRenderer>().sprite = thunderboltSprite; // Assign thunderbolt sprite to attack
        attack.transform.localScale = new Vector3(1.4f, 1.4f, 1f); // Make projectile bigger

        // Move projectiles up
        Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.linearVelocity = Vector2.up * projSpeed;
        }

        Destroy(attack, 1.5f); // Delete attack object after certain time
    }

    void SpacebarAttack() { // Space bar to send projectiles
        PlayerSpawnProj();
        animator.SetBool("IsWalking", false); // Face enemies when attacking
    }

    void ClickAttack() { // Click to send projectiles
        PlayerSpawnProj();
        animator.SetBool("IsWalking", false); // Face enemies when attacking
    }

}


