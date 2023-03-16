using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Holds the score inside the game
/// </summary>
public class ScoreModule
{
    readonly string HIGH_SCORE_KEY = "***HIGH SCORE***";
    int score;
    public int Score
    {
        get => score;
    }
    public int HighScore
    {
        get
        {
            if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
            {
                return PlayerPrefs.GetInt(HIGH_SCORE_KEY);
            }
            return 0;       
        }

        set {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY,value);
        }
    }

    public void ValidateHighScore()
    {
        if(score > HighScore)
        {
            HighScore = score;
        }
    }

    public void AddPoints(int point)
    {
        score += point;
    }
    public void ResetScore()
    {
        score = 0;
    }
}

/// <summary>
/// Handles game mechanism and workflow
/// </summary>
public class GameManager : BaseGameComponent, IGameManager
{
    readonly float RespawnDelay = 2f;

    public static event Action OnGameReset;


    [SerializeField]
    GameConfig gameConfig;

    private bool isGameStarted;
    public GameConfig GameConfig { get => gameConfig; }
    public bool IsGameStarted { get => isGameStarted; }

    ScoreModule scoreModule  = new ScoreModule();

    public int HighScore { get => scoreModule.HighScore; }

    public int Score { get => scoreModule.Score; }

    public int Life
    {
        get
        {
            if(_playerManager == null || _playerManager.PlayerShip == null)
            {
                throw new NullReferenceException("Life cannot be obtained due to null exception");
            }
            return _playerManager.PlayerShip.Health;
        }

    }

    // Start is called before the first frame update
    IAsteroidController _asteroidController;
    IPlayerManager _playerManager;
    IUIManager _uiManager;
    IAudioManager _audioManager;
    IPowerUpController _powerUpManager;


    public override void Init(IGameComponentManager scenemanager)
    {
        base.Init(scenemanager);
        _asteroidController = scenemanager.GetSceneComponent<IAsteroidController>();
        _playerManager = scenemanager.GetSceneComponent<IPlayerManager>();
        _uiManager = scenemanager.GetSceneComponent<IUIManager>();
        _audioManager = scenemanager.GetSceneComponent<IAudioManager>();
        _powerUpManager = scenemanager.GetSceneComponent<IPowerUpController>();

        if (gameConfig == null)
        {
            throw new NullReferenceException("Game Config is missing");
        }
    }

    /// <summary>
    /// Funtion to start/reset the game
    /// </summary>
    /// <param name="isNewGame"></param>
    public void StartGame(bool isNewGame = false)
    {
        isGameStarted = true;

        if (isNewGame)
        {
            scoreModule.ResetScore();
            _audioManager.PlayGameBGM();
        }

        _playerManager.StartPlayer(isNewGame);
        _asteroidController.SpawnEnemies();
        _powerUpManager.StartSpawningPowerUps();

        
    }

    /// <summary>
    ///Call when player lost a life 
    /// </summary>
    public void LostLife()
    {
        AudioEventHandler.Instance.PlaySFX(_playerManager.PlayerShip.Config.deathSFX);

        isGameStarted = false;
        OnGameReset?.Invoke(); 
        scoreModule.ValidateHighScore();

        _uiManager.OnLifeLost();

        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {     
        yield return new WaitForSeconds(RespawnDelay);
        StartGame();
    }

    /// <summary>
    /// Call when the game meets the condition of gameover
    /// </summary>
    public void GameOver()
    {
        AudioEventHandler.Instance.PlaySFX(_playerManager.PlayerShip.Config.gameOverSFX);

        isGameStarted = false;
        scoreModule.ValidateHighScore();

        OnGameReset?.Invoke();
        _uiManager.ShowGameOverUI();

     
        _audioManager.StopGameBGM();

    }

    /// <summary>
    /// Update the score in the game
    /// </summary>
    /// <param name="points"></param>
    public void UpdateScore(int points)
    {
        if (isGameStarted)
        {
            scoreModule.AddPoints(points);
            _uiManager.UpdatePoints(scoreModule.Score);
        }  
        
    }
}


public interface IGameManager : IBaseGameComponent
{
    bool IsGameStarted { get; }
    GameConfig GameConfig { get; }
    int  HighScore { get; }
    int Life { get; }
    int Score { get; }

    void UpdateScore(int points);

    void StartGame(bool isNewGame = false);
    void LostLife();
    void GameOver();

}