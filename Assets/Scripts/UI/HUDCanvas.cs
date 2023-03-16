using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Canvas that shows at the gameplay screen
/// </summary>
public class HUDCanvas : UICanvas
{
    [SerializeField]
    TextMeshProUGUI scoreField,highScoreField;

    [SerializeField]
    Transform lifeContainer;

    [SerializeField]
    GameObject lifeIdicationPrefab;

    public void DisplayTotalLife(int totalLife)
    {
        if (lifeContainer.childCount < totalLife)
        {
            for (int i = lifeContainer.childCount; i < totalLife; i++)
            {
                Instantiate(lifeIdicationPrefab, lifeContainer);
            }
        }
        foreach(Transform child in lifeContainer)
        {
            child.gameObject.SetActive(true);
        }

    }

    public void OnLifeLost(int remainingLife)
    {
        for(int i = remainingLife; i< lifeContainer.childCount; i++)
        {
            Transform child = lifeContainer.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                break;
            }
        }
    }

    public void DisplayHighScore(int highScore)
    {

        highScoreField.text = highScore.ToString();

    }

    public void DisplayScore(int score)
    {
        scoreField.text = score.ToString();
    }
}
