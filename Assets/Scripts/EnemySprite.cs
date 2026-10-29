using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    private Rigidbody2D rb;

    // Animation
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Sprite genericAttackSprite;

    // Movement
    private float speed;
    private float xStart;
    private float yStart;
    private float offset;

    // Projectiles
    public GameObject projPrefab;
    public Transform projSpawnLoc;

    // Death
    private bool isDead = false;

    [Header("Movement Settings")]
    public float minSpeed = 1f;
    public float maxSpeed = 1f;
    public float moveRange = 2f;

    [Header("Shooting Settings")]
    public float minAttackDelay = 3f;
    public float maxAttackDelay = 7f;
    public float projSpeed = 7f;

    [Header("Enemy stats")]
    public float health = 3f;
    public float dmg = 1f;

    [Header("Boss Settings")]
    public bool isBoss = false;
    public float bossScore = 1000;

    [Header("Audio")]
    public AudioClip getHitSound;
    public AudioClip deathSound;

    public enum MovePattern { // Save different types of movement patterns
        Horizontal,
        Circular
    }

    public MovePattern movePattern;

    // Each wave should have their own random pattern
    private static MovePattern currWavePattern; // Used to make all enemies share move pattern
    private static bool wavePatternChosen = false;

    public static void NewWavePattern() {
        currWavePattern = (MovePattern)Random.Range(0, 2);
        wavePatternChosen = true;
    }
    
    void Start() // Allow enemies to move and start shooting as game starts
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get Animator component

        // Choose move pattern for all enemy wave instances if not already chosen
        if (!wavePatternChosen) {
            NewWavePattern();
        }

        movePattern = currWavePattern;

        speed = Random.Range(minSpeed, maxSpeed); // Calculate random speed
        xStart = transform.position.x; // Starting position
        yStart = transform.position.y;
        offset = Random.Range(0f, 10f); // Calculate random distance

        // Attack
        StartCoroutine(RandomAttack());
    }

    void Update()
    {
        if (isDead) {
            return;
        }

        // Different movement patterns
        float t = (Time.time + offset) * speed;
        float x = xStart;
        float y = yStart;

        switch(movePattern) {
            case MovePattern.Horizontal:
                x = xStart + Mathf.Sin(t) * moveRange;
                break;

            case MovePattern.Circular:
                x = xStart + Mathf.Cos(t) * moveRange;
                y = yStart + Mathf.Sin(t) * moveRange;
                break;
        }

        // // Random horizontal movement of all enemies; not in sync
        // float x = xStart + Mathf.Sin((Time.time + offset) * speed) * moveRange; // Calculate movement path
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void TakeDamage(float damage) { // Enemy takes damage
        if (isDead) { // Prevents double deaths
            return;
        }
        
        health -= damage;
        health = Mathf.Max(health, 0); // Make sure health does not go below 0

        if (health <= 0) { // Death
        AudioManager.instance.PlaySound(deathSound, 1f); // Audio when enemy dies
            if (isBoss) {
                isDead = true;
                StopAllCoroutines();
                if (animator != null) {
                    animator.SetTrigger("Die");
                }
            }
            else {
                FindAnyObjectByType<GameManager>().RegisterEnemyDestroyed(); // - enemy
                
                Destroy(gameObject);
                ScoreManager.instance.AddScore(100); // Add points when enemy is defeated
            }            
            return;
        }

        AudioManager.instance.PlaySound(getHitSound, 1f); // Audio when enemy takes damage

        if (animator != null) { // Play animation if not fatal hit
            animator.ResetTrigger("Hurt"); // If hits occur close together
            animator.SetTrigger("Hurt");
        }
    }

    public void FinishDeath() { // Used for boss death
        ScoreManager.instance.AddScore((int)bossScore);

        if (isBoss) { // Go to victory screen
            FindAnyObjectByType<GameManager>().Victory();
        }

        Destroy(gameObject);
    }

    IEnumerator RandomAttack() {
        while (true) {
            float delay = Random.Range(minAttackDelay, maxAttackDelay);
            yield return new WaitForSeconds(delay);

            if ((isBoss) && (animator != null)) {
                animator.SetTrigger("Attack");
            }
            
            // Adjust projectile count depending on enemy
            int projectileCount = isBoss ? 3 : 1; // 3 Projectiles if boss, 1 if not

            for (int i = 0; i < projectileCount; i++) {
                Vector3 spawnPos = projSpawnLoc.position;
                if (isBoss) { // Space out boss attacks
                    spawnPos.x += (i - 1) * 1.5f;
                }

                GameObject attack = Instantiate(projPrefab, spawnPos, Quaternion.identity);

                attack.GetComponent<EnemyProjectile>().dmg = dmg; // Give projectile dmg value of enemy
                attack.layer = LayerMask.NameToLayer("EnemyProj");
                attack.GetComponent<SpriteRenderer>().sprite = genericAttackSprite; // Assign sprite to projectile
                
                float size = isBoss ? 0.5f : 0.2f; // 0.5f if boss, 0.2f if not
            
                attack.transform.localScale = new Vector3(size, size, 1f);
                
                // Move projectiles down
                Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
                if (rb != null) {
                    rb.linearVelocity = Vector2.up * -projSpeed;
                }

                Destroy(attack, 1.5f); // Delete attack object after certain time
            }
        }
    }
}
