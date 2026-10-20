using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject projPrefab;
    public Transform projSpawnLoc;

    [Header("Inscribed")]
    public float moveSpeed = 3f;
    public float changeDirTime = 2f;
    public float moveRange = 2f;

    [Header("Shooting Settings")]
    public float minAttackDelay = 3f;
    public float maxAttackDelay = 7f;
    public float projSpeed = 7f;
    
    // Allow enemies to start shooting as game starts
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(randomAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
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
