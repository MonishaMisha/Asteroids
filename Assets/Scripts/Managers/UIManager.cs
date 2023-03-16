using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the workflow of UI
/// </summary>
public class UIManager : BaseGameComponent,IUIManager
{
    [SerializeField]
    HomeCanvas homeCanvas;

    [SerializeField]
    GameOverCanvas gameoverCanvas;

    [SerializeField]
    HUDCanvas hudCanvas;


    [SerializeField]
    MobileInputCanvas mobileInputCanvas;

    UICanvas activeCanvas;
    IGameManager _gameManager;
    public override void Init(IGameComponentManager scenemanager)
    {
        base.Init(scenemanager);
        _gameManager = scenemanager.GetSceneComponent<IGameManager>();

       

        homeCanvas?.Init(this);
        gameoverCanvas?.Init(this);
        hudCanvas?.Init(this);
        mobileInputCanvas.Init(this);

        DisableAllCanvas();


        ActivateCanvas(homeCanvas);


        homeCanvas.DisplayHighScore(_gameManager.HighScore);
    }

    private void DisableAllCanvas()
    {
        gameoverCanvas?.SetActive(false);
        hudCanvas ?.SetActive(false);
        mobileInputCanvas?.SetActive(false);
    }

    public void OnStartGameAction()
    {
        ActivateCanvas(hudCanvas);
        _gameManager.StartGame(true);

        UpdateHUDCanvas();

#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
        //Shows the mobile controller panel
        mobileInputCanvas.SetActive(true);
#endif

    }

    public void ShowGameOverUI()
    {
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)

        mobileInputCanvas.SetActive(false);
#endif
        ActivateCanvas(gameoverCanvas);
        gameoverCanvas.UpdateFinalScore(_gameManager.Score,_gameManager.HighScore);
    }

    public void UpdatePoints(int score)
    {
        hudCanvas.DisplayScore(score);
    }

    void ActivateCanvas(UICanvas canvas)
    {
        activeCanvas?.SetActive(false);

        canvas.SetActive(true);

        activeCanvas = canvas;
    }
    
    public void OnExitGame()
    {
        ActivateCanvas(homeCanvas);
        homeCanvas.DisplayHighScore(_gameManager.HighScore);
    }
    public void OnRestartGame()
    {
        OnStartGameAction();
    }

    void UpdateHUDCanvas()
    {
        hudCanvas.DisplayScore(_gameManager.Score);
        hudCanvas.DisplayTotalLife(_gameManager.Life);
        hudCanvas.DisplayHighScore(_gameManager.HighScore);
    }

    public void OnLifeLost()
    {
        hudCanvas.OnLifeLost(_gameManager.Life);
    }

    // Start is called before the first frame update
}

public interface IUIManager : IBaseGameComponent
{
    void OnStartGameAction();

    void UpdatePoints(int score);

    void OnExitGame();

    void OnLifeLost();

   void OnRestartGame();

    void ShowGameOverUI();

}