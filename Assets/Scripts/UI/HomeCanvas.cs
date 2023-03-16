using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Canvas that shows at Home scene
/// </summary>
public class HomeCanvas : UICanvas
{

    [SerializeField]
    TextMeshProUGUI highscoreText;
    [SerializeField]
    Button playButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnGameStartClicked);
    }

    private void OnGameStartClicked()
    {
        manager.OnStartGameAction();
    }

    public void DisplayHighScore(int score)
    {
        highscoreText.text = score.ToString(); ;
    }
  
}
