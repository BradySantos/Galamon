using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    private Rigidbody2D rb;
    // For movement
    private float speed;
    private float xStart;
    private float yStart;
    private float offset;

    public GameObject projPrefab;
    public Transform projSpawnLoc;

    [Header("Movement Settings")]
    public float minSpeed = 1f;
    public float maxSpeed = 1f;
    public float moveRange = 2f;

    [Header("Shooting Settings")]
    public float minAttackDelay = 3f;
    public float maxAttackDelay = 7f;
    public float projSpeed = 7f;

    [Header("Enemy stats")]
    public float health = 3f; // TODO: Make sure these are different depending on what spawns
    public float dmg = 1f;

    // [Header("Audio")] // TODO: Add audio
    // public AudioSource audioSource;

    // public AudioClip ...;
    // public AudioClip ...;
    // public AudioClip ...;

    public enum MovePattern { // Save different types of movement patterns
        Horizontal,
        Vertical,
        Circular
    }

    public MovePattern movePattern;
    private static MovePattern groupPattern; // Used to make all enemies share move pattern
    private static bool patternChosen = false;
    
    void Start() // Allow enemies to move and start shooting as game starts
    {
        rb = GetComponent<Rigidbody2D>();

        // Movement; choose pattern for all enemy instances if not already chosen
        if (!patternChosen) {
            groupPattern = (MovePattern)Random.Range(0,3); // Select random movement pattern
            patternChosen = true;
        }

        movePattern = groupPattern;

        speed = Random.Range(minSpeed, maxSpeed); // Calculate random speed
        xStart = transform.position.x; // Starting position
        yStart = transform.position.y;
        offset = Random.Range(0f, 10f); // Calculate random distance

        // Attack
        StartCoroutine(RandomAttack());
    }

    void Update()
    {
        // Different movement patterns
        float t = (Time.time + offset) * speed;
        float x = xStart;
        float y = yStart;

        switch(movePattern) {
            case MovePattern.Horizontal:
                x = xStart + Mathf.Sin(t) * moveRange;
                break;

            case MovePattern.Vertical:
                y = yStart + Mathf.Sin(t) * moveRange;
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
        health -= damage;

        if (health <= 0) { // Death
            Destroy(gameObject);
        }
        // TODO: Update screen to next level when all are destroyed
    }

    IEnumerator RandomAttack() {
        while (true) {
            float delay = Random.Range(minAttackDelay, maxAttackDelay);
            yield return new WaitForSeconds(delay);

            GameObject attack = Instantiate(projPrefab, projSpawnLoc.position, Quaternion.identity);
            attack.GetComponent<EnemyProjectile>().dmg = dmg; // Give projectile dmg value of enemy
            attack.layer = LayerMask.NameToLayer("EnemyProj");

            // Move projectiles down
            Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.linearVelocity = Vector2.up * -projSpeed;
            }

            Destroy(attack, 1.5f); // Delete attack object after certain time
        }
    }

}
