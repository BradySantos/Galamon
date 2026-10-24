using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // [Header("Audio")] // TODO: Add audio
    // public AudioSource audioSource;

    // public AudioClip ...;
    // public AudioClip ...;
    // public AudioClip ...;

    void OnTriggerEnter2D(Collider2D coll) { // Handle collisions on enemy end
        if ((gameObject.layer == LayerMask.NameToLayer("EnemyProj")) && (coll.CompareTag("Player"))) {
            Destroy(gameObject);
        
            // TODO: Play sound when enemy is damaged
            // PlaySound(...)

            // Update health
            // scoreCounter.score += 100;
            // updateStats();
        }
    }
}
