using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public enum PowerUpType
{
    Shield,
    CresentShot
}

[Serializable]
public struct PowerUpInfo
{
    public PowerUpType powerUpType;
    public float coolDownTime;
    public GameObject powerUpPrefab;
}

/// <summary>
/// Controls the poweup entity
/// </summary>
public class PowerUpController : BaseGameComponent, IPowerUpController
{

    //Controls the spawning of powerups
    //Controld interval of spanning
    //Check if player already recieved powerup if not spawn
    //Controls cooldown period of powerup

    private List<IPowerUp> powerUpMap = new List<IPowerUp>();
    [SerializeField]
    private PowerUpConfig config;
    private IPlayerManager _playerManager;
    WaitForSeconds wait;
    private Action OnResetCall;
    private Action OnCoolDownCall;
    AutoTimeOutQueue autoTimeOutQueue;

    Coroutine spawningRoutine;

    private void OnEnable()
    {
        GameManager.OnGameReset += OnGameReset;
    }

    private void OnGameReset()
    {

        StopAllCoroutines();
        spawningRoutine = null;
        OnResetCall?.Invoke();
        autoTimeOutQueue.Dispose();
    }

    /// <summary>
    /// Starts spawning powerups
    /// </summary>
    public void SpawnPowerps()
    {   
        var rnd = new Random();
        var randomized = powerUpMap.OrderBy(item => rnd.Next());
        var powerUp = randomized.FirstOrDefault(x => !_playerManager.AvailablePowerUps.Contains(x.PowerUpType));
        if (powerUp != null)
        {
            Vector3 pos = PositionHelper.GetRandomSpawningPointInsideScreen(2, 2);
            Vector2 dir = (_playerManager.PlayerShip.Transform.position - pos).normalized;
            powerUp.Initialize(pos);
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (_playerManager.AvailablePowerUps.Count() < config.powerUpInfos.Length)
        {
            yield return wait;
            SpawnPowerps();
        }
    }
    private void OnDisable()
    {
        GameManager.OnGameReset -= OnGameReset;
    }


    public override void Init(IGameComponentManager scenemanager)
    {
        base.Init(scenemanager);
        _playerManager = scenemanager.GetSceneComponent<IPlayerManager>();
        wait = new WaitForSeconds(config.occuranceDelay);
        Initialize();
        autoTimeOutQueue = new AutoTimeOutQueue();
    }

    private void Initialize()
    {
        foreach (var powerUpInfo in config.powerUpInfos)
        {
           powerUpMap.Add(CreatePowerUps(powerUpInfo));
        }
    }

    private IPowerUp CreatePowerUps(PowerUpInfo info)
    {
        var instance = Instantiate(info.powerUpPrefab);
        var powerUp = instance.GetComponent<IPowerUp>();
        OnResetCall += () => { ResetAsteroid(powerUp); };
        powerUp.RegisterInteractionCallback(OnPowerUpInteraction);
        powerUp.SetType(info.powerUpType);
        powerUp.SetCoolDownTime(info.coolDownTime);
        powerUp.SetActive(false);
        return powerUp;
    }

    /// <summary>
    /// Handles the power up interaction
    /// </summary>
    /// <param name="powerUp"></param>
    private void OnPowerUpInteraction(IPowerUp powerUp)
    {
        ResetAsteroid(powerUp);
        if (spawningRoutine != null)
        {
            StopCoroutine(spawningRoutine);
            spawningRoutine = null;
        }
        _playerManager.SetPowerUp(powerUp.PowerUpType);
        spawningRoutine = StartCoroutine(SpawnRoutine());
        autoTimeOutQueue.Add(powerUp.CoolDownTime,()=> {

            OnPowerUpFinished(powerUp);        
        },this);


    }

    /// <summary>
    /// Calls when a powerup is finished
    /// </summary>
    /// <param name="powerUp"></param>
    private void OnPowerUpFinished(IPowerUp powerUp)
    {
        _playerManager.ReleasePowerUp(powerUp.PowerUpType);
        if(spawningRoutine == null)
        {
            StartSpawningPowerUps();
        }
    }

    private void ResetAsteroid(IPowerUp powerUp)
    {
        powerUp.SetActive(false);
     }

    /// <summary>
    /// Starts to spawn power ups
    /// </summary>
    public void StartSpawningPowerUps()
    {
        spawningRoutine = StartCoroutine(SpawnRoutine());
    }
}

public interface IPowerUpController : IBaseGameComponent
{
    void StartSpawningPowerUps();
}