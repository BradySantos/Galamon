using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Enemy Grid")]
    public int rows = 2;
    public int cols = 7;
    public float xSpacing = 1f;
    public float ySpacing = 1f;

    [Header("Enemy Types")]
    public GameObject[] enemyPrefabs;

    public void SetEnemyTypes(GameObject[] newPrefabs) { // Manages enemy types
        enemyPrefabs = newPrefabs;
    }

    public void SpawnEnemies() { // Manages enemy spawns
        if ((enemyPrefabs == null) || (enemyPrefabs.Length == 0)) {
            return;
        }

        // If boss, only spawn one
        bool isBossLevel = false;
        GameObject bossPrefab = null;

        foreach (GameObject prefab in enemyPrefabs) { // Check if the prefabs are boss prefab
            if (prefab != null) {
                EnemySprite enemyScript = prefab.GetComponent<EnemySprite>();
                if ((enemyScript != null) && (enemyScript.isBoss)) {
                    isBossLevel = true;
                    bossPrefab = prefab;
                    break;
                }
            }
        }

        // Spawn one boss
        if (isBossLevel && (bossPrefab != null)) {
            Camera cam = Camera.main; // Get camera settings to help center boss
            float xScreen = cam.transform.position.x;
            float yScreen = cam.transform.position.y + 2f;

            Vector3 spawnPos = new Vector3(xScreen, yScreen, 0f);

            GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            FindAnyObjectByType<GameManager>().RegisterEnemySpawned();
            return; // Return before spawning whole grid
        }

        for (int i = 0; i < rows; i++) { // Rows
            for (int j = 0; j < cols; j++) { // Columns
                Vector3 spawnPos = transform.position + new Vector3(j * xSpacing, -i * ySpacing, 0);

                int randomIndex = Random.Range(0, enemyPrefabs.Length); // Randomize opponents

                GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

                FindAnyObjectByType<GameManager>().RegisterEnemySpawned(); // + enemy
            }
        }
    }
}
