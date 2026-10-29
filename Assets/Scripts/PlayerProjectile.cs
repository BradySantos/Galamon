using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float dmg; // Takes on player damage value when proj instantiated

    void OnTriggerEnter2D(Collider2D coll) { // Handle collisions on player end
        if ((gameObject.layer == LayerMask.NameToLayer("PlayerProj")) && (coll.CompareTag("Enemy"))) {
            Destroy(gameObject);

            // Update enemy health
            EnemySprite enemy = coll.GetComponent<EnemySprite>();

            if (enemy != null) {
                enemy.TakeDamage(dmg);
            }
        }
    }
}
