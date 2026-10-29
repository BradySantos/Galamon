using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Enemy Grid")]
    public int rows = 2;
    public int cols = 5;
    public float xSpacing = 1f;
    public float ySpacing = 1f;

    // Spawn enemies
    void Start()
    {
        for (int i = 0; i < rows; i++) { // Rows
            for (int j = 0; j < cols; j++) { // Columns
                Vector3 spawnPos = transform.position + new Vector3(j * xSpacing, -i * ySpacing, 0);

                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
