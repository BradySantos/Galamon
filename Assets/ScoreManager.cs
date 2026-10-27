using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;
    public TMP_Text scoreText;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        score = 0; // Reset score on start
        UpdateScore();
    }

    public void AddScore(int points) {
        score += points;
        UpdateScore();
    }

    void UpdateScore() {
        scoreText.text = "Score: " + score;
    }
}
