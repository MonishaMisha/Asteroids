using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Panel that shows on gameover
/// </summary>
public class GameOverCanvas : UICanvas
{
    [SerializeField]
    TextMeshProUGUI highscoreText,scoreText;
    [SerializeField]
    Button restartButton,exitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartGameClicked);
        exitButton.onClick.AddListener(OnExitGameClicked);
    }

    private void OnExitGameClicked()
    {
        manager.OnExitGame();
    }

    private void OnRestartGameClicked()
    {
        manager.OnRestartGame();
    }


    public void UpdateFinalScore(int score, int highScore)
    {
        highscoreText.text = highScore.ToString();
        scoreText.text = score.ToString();

    }



}
