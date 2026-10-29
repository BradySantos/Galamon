using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;
    public int highScore = 0;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    void Awake() {
        instance = this;

        // Load saved highscore
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Start()
    {
        score = 0; // Reset score on start
        UpdateScore();
    }

    public void AddScore(int points) {
        score += points;

        // Check if score is higher than saved high score
        if (score > highScore) {
            highScore = score;

            // Save high score
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateScore();
    }

    void UpdateScore() {
        if (scoreText != null) {
            scoreText.text = "Score: " + score;
        }

        if (highScoreText != null) {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    [Tooltip("Reset HighScore in PlayerPrefs")]
    public bool resetHighScoreNow = false;

    void OnValidate() {
        if (resetHighScoreNow) {
            resetHighScoreNow = false;
            PlayerPrefs.SetInt("HighScore", 0);
            Debug.LogWarning("PlayerPrefs HighScore reset to 0.");
        }
    }
}
