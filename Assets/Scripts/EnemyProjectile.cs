using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float dmg; // Takes on enemy damage value when proj instantiated

    void OnTriggerEnter2D(Collider2D coll) { // Handle collisions on enemy end
        if ((gameObject.layer == LayerMask.NameToLayer("EnemyProj")) && (coll.CompareTag("Player"))) {
            Destroy(gameObject);
        
            // Update player health
            PlayerSprite player = coll.GetComponent<PlayerSprite>();

            if (player != null) {
                player.TakeDamage(dmg);
            }
        }
    }
}
