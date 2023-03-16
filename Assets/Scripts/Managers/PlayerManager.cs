using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manage player workflow
/// </summary>
public class PlayerManager : BaseGameComponent, IPlayerManager
{
    // Used as a medium between player and game manager
    public static event Action<PowerUpType> OnPowerUpAquired;
    public static event Action<PowerUpType> OnPowerUpLost;
    [SerializeField]
    GameObject playerPrefab;
    private IPlayerShip playerShip;

    private HashSet<PowerUpType> availablePowerUps = new HashSet<PowerUpType>();

    public IPlayerShip PlayerShip { get => playerShip; }

    public IEnumerable<PowerUpType> AvailablePowerUps => availablePowerUps;

    public bool hasPowerUp => availablePowerUps.Any();

    private IGameManager _gameManager;
    public override void Init(IGameComponentManager scenemanager)
    {
        base.Init(scenemanager);
        _gameManager = scenemanager.GetSceneComponent<IGameManager>();
    }


    /// <summary>
    /// Used to create/spawn player  
    /// </summary>
    /// <param name="isNewGame"></param>
    public void StartPlayer(bool isNewGame = false)
    {
        if (playerShip == null)
        {
            GameObject gameobject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            playerShip = gameobject.GetComponent<IPlayerShip>();
            playerShip.AddManagerDependency(this);
            playerShip.OnInitialize();
        }
        else
        {
            playerShip.ResetPlayer(isNewGame);
        }
    }


    public void PlayerDestroyed()
    {
        if (playerShip.Health > 0)
        {
            _gameManager.LostLife();
        }
        else
        {
            _gameManager.GameOver();
        }
        availablePowerUps.Clear();
    }

    public void SetPowerUp(PowerUpType type)
    {
        availablePowerUps.Add(type);
        OnPowerUpAquired?.Invoke(type);
    }

    public void ReleasePowerUp(PowerUpType type)
    {
        if (availablePowerUps.Contains(type))
        {
            availablePowerUps.Remove(type);
        }
        OnPowerUpLost?.Invoke(type);
    }
}


public interface IPlayerManager : IBaseGameComponent
{
    void StartPlayer(bool isNewGame = false);
    bool hasPowerUp { get; }
    IPlayerShip PlayerShip { get; }
    IEnumerable<PowerUpType> AvailablePowerUps { get; }
    void SetPowerUp(PowerUpType type);
    void ReleasePowerUp(PowerUpType type);
    void PlayerDestroyed();


}