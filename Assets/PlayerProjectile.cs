using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // [Header("Audio")] // TODO: Add audio
    // public AudioSource audioSource;

    // public AudioClip ...;
    // public AudioClip ...;
    // public AudioClip ...;

    void OnTriggerEnter2D(Collider2D coll) { // Handle collisions on player end
        if ((gameObject.layer == LayerMask.NameToLayer("PlayerProj")) && (coll.CompareTag("Enemy"))) {
            Destroy(gameObject);

            // TODO: Play sound when player is damaged
            // PlaySound(...);

            // Update health and score
            // updateStats();
        }
    }

}
