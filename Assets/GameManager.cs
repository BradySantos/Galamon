using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startMenuPanel;
    public GameObject gameOverPanel;

    void Start() { // Set main menu panel and hide game over panel first
        startMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f; // Pause gameplay during start menu
    }

    public void StartGame() { // Hide main menu panel 
        startMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Resume gamemplay
    }

    public void GameOver() { // Set game over panel 
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause gameplay during game over menu
    }

    public void ReturnMenu() { // Return to start menu 
        startMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f; // Pause gameplay during start menu
    }

    public void RestartGame() {
        Time.timeScale = 1f; // Resume gameplay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload game scene
    }
}
