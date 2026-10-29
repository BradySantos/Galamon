using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startMenuPanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Audio")] 
    public AudioClip startButtonSound;
    public AudioClip returnButtonSound;
    public AudioClip victorySound;

    [Header("Backgrounds")]
    public SpriteRenderer backgroundRenderer;
    public Sprite[] backgrounds;

    void Start() { // Set main menu panel and hide game over panel first
        startMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f; // Pause gameplay during start menu
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

    public void StartGame() { // Hide main menu panel 
        AudioManager.instance.PlaySound(startButtonSound, 0.7f, 0.3f); // Start button audio

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
}
