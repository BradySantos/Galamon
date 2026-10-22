using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    private Rigidbody2D rb;
    //For movement
    private float speed;
    private float xStart;
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
    
    // Allow enemies to move and start shooting as game starts
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Movement
        speed = Random.Range(minSpeed, maxSpeed); // Calculate random speed
        xStart = transform.position.x; // Starting position
        offset = Random.Range(0f, 10f); // Calculate random distance

        // Attack
        StartCoroutine(randomAttack());
    }

    void Update()
    {
        // !!!!!! Add Enemy patterns


        // Movement
        float x = xStart + Mathf.Sin((Time.time + offset) * speed) * moveRange; // Calculate movement path
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    IEnumerator randomAttack() {
        while (true) {
            float delay = Random.Range(minAttackDelay, maxAttackDelay);
            yield return new WaitForSeconds(delay);

            GameObject attack = Instantiate(projPrefab, projSpawnLoc.position, Quaternion.identity);

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
