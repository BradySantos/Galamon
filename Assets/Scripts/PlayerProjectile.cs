using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float dmg; // Takes on player damage value when proj instantiated
    // [Header("Audio")] // TODO: Add audio
    // public AudioSource audioSource;

    // public AudioClip ...;
    // public AudioClip ...;
    // public AudioClip ...;

    void OnTriggerEnter2D(Collider2D coll) { // Handle collisions on player end
        if ((gameObject.layer == LayerMask.NameToLayer("PlayerProj")) && (coll.CompareTag("Enemy"))) {
            Destroy(gameObject);

            // Update enemy health
            EnemySprite enemy = coll.GetComponent<EnemySprite>();

            if (enemy != null) {
                enemy.TakeDamage(dmg);
            }

            // TODO: Play sound when player is damaged
            // PlaySound(...);

            // Update health and score
            // updateStats();
        }
    }

}
