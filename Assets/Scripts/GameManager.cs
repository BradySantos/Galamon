using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class LevelSet {
    public GameObject[] enemyPrefabs;
}

public class GameManager : MonoBehaviour
{
    public GameObject startMenuPanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    // Track enemies remaining
    private int enemiesRemaining = 0;
    private bool levelTransition = false;

    [Header("Audio")] 
    public AudioClip startButtonSound;
    public AudioClip returnButtonSound;
    public AudioClip victorySound;

    [Header("Level UI")]
    public TMP_Text levelText;

    [Header("Level Settings")]
    public int currentLevel = 1;
    public LevelSet[] levelSets;
    public SpriteRenderer backgroundRenderer;
    public Sprite[] backgrounds;
    public EnemySpawner enemySpawner;

    void Start() { // Set main menu panel and hide game over panel first
        startMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        
        Time.timeScale = 0f; // Pause gameplay during start menu
    }

    public void StartGame() { // Hide main menu panel 
        AudioManager.instance.PlaySound(startButtonSound, 0.7f, 0.3f); // Start button audio
        
        UpdateLevel();

        EnemySprite.NewWavePattern(); // Determine move pattern of wave
        enemySpawner.SpawnEnemies(); // Spawn enemies on start

        startMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Resume gamemplay
    }

    public void GameOver() { // Set game over panel 
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause gameplay during game over menu
    }

    public void Victory() {
        AudioManager.instance.PlaySound(victorySound, 1f);

        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame() {
        AudioManager.instance.PlaySound(returnButtonSound, 1f); // Return to main menu audio

        Time.timeScale = 1f; // Resume gameplay
        StartCoroutine(RestartAfterDelay());
    }

    IEnumerator RestartAfterDelay() { // Create delay so the game can play audio before resetting scene
        yield return new WaitForSecondsRealtime(2.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload game scene
    }

    public void changeBackground(int index) { // Change game background (not menu backgrounds)
        // Out of bounds indexes
        if (backgrounds.Length == 0) {
            return;
        }
        if ((index < 0) || (index >= backgrounds.Length)) {
            return;
        }

        backgroundRenderer.sprite = backgrounds[index];
    }

    public void UpdateLevel() { // Manage levels
        // If nonexistent sets/bg
        if ((levelSets == null) || (levelSets.Length == 0) || (backgrounds == null) || (backgrounds.Length == 0)) { 
            UpdateLevelText();
            return;
        }
        
        int setIndex = (currentLevel - 1) / 2;

        changeBackground(setIndex);

        enemySpawner.SetEnemyTypes(levelSets[setIndex].enemyPrefabs);

        UpdateLevelText();
    }

    public void UpdateLevelText() { // Level text
        if (levelText != null) {
            levelText.text = "Level " + currentLevel;
        }
    }

    public void RegisterEnemySpawned() { // + enemies
        enemiesRemaining++;
    }

    public void RegisterEnemyDestroyed() { // - enemies 
        enemiesRemaining--;

        if ((enemiesRemaining <= 0) && (!levelTransition)) {
            levelTransition = true;
            StartCoroutine(AdvanceLevelAfterDelay());
        }
    }

    IEnumerator AdvanceLevelAfterDelay() { // Delay between waves
        yield return new WaitForSeconds(1.5f);

        currentLevel++;

        UpdateLevel();

        enemiesRemaining = 0;
        levelTransition = false;

        EnemySprite.NewWavePattern(); // Update wave pattern
        enemySpawner.SpawnEnemies();
    }
}
