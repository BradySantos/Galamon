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

    [Header("Enemy Types")]
    public GameObject[] enemyPrefabs;

    // Spawn enemies
    void Start()
    {
        for (int i = 0; i < rows; i++) { // Rows
            for (int j = 0; j < cols; j++) { // Columns
                Vector3 spawnPos = transform.position + new Vector3(j * xSpacing, -i * ySpacing, 0);

                int randomIndex = Random.Range(0, enemyPrefabs.Length); // Randomize opponents

                Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);
            }
        }
    }
}
