using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// Manage the asteroids spawning in the scene
/// </summary>
public class AsteroidController : BaseGameComponent, IAsteroidController
{

    [SerializeField]
    List<GameObject> prefabSet;
    [SerializeField]
    ParticleSystem particleSystemPrefab;

    Dictionary<AsteroidType, GameObject> prefabMap = new Dictionary<AsteroidType, GameObject>();

    /// <summary>
    /// contains all the large asteroids
    /// </summary>
    List<IAsteroid> asteroidCollection = new List<IAsteroid>();


    private Action OnResetCall;

    IPlayerManager _playerManager;
    IGameManager _gameManager;

    WaitForSeconds wait;

    bool isSpawingAsteroids = false;

    private void OnEnable()
    {
        GameManager.OnGameReset += OnGameReset;
    }
    private void OnDisable()
    {
        GameManager.OnGameReset -= OnGameReset;
    }

    private void OnGameReset()
    {
        isSpawingAsteroids = false;
        StopAllCoroutines();

        asteroidCollection.Clear();
        OnResetCall?.Invoke();
    }

    public override void Init(IGameComponentManager scenemanager)
    {
        base.Init(scenemanager);
        Intialize();
        _playerManager = scenemanager.GetSceneComponent<IPlayerManager>();
        _gameManager = scenemanager.GetSceneComponent<IGameManager>();

        wait = new WaitForSeconds(_gameManager.GameConfig.AsteroidSpawningInterval);
    }

    private void Intialize()
    {
        foreach (GameObject asteroid in prefabSet)
        {
            IAsteroid _asteroid = asteroid.GetComponent<IAsteroid>();
            prefabMap.Add(_asteroid.Config.asteroidType, asteroid);
        }
        prefabSet.Clear();
    }

    /// <summary>
    /// Used to create/spawn Asteroids 
    /// </summary>
    public void SpawnEnemies()
    {

        if (_gameManager.GameConfig.TotalLargeAsteroids > 0)
        {

            if (asteroidCollection.Count == 0)
            {
                for (var i = 0; i < _gameManager.GameConfig.TotalLargeAsteroids; i++)
                {
                    CreateAsteroid();
                }
            }
            StartCoroutine(SetupLargeAsteroids());
        }
    }



    void ResetAsteroid(IAsteroid asteroid)
    {
        asteroid.SetActive(false);
        asteroid.UnMark();
        // AddToCollection(asteroid);
    }

    void AddToCollection(IAsteroid asteroid)
    {
        AsteroidType type = asteroid.Config.asteroidType;
        asteroidCollection.Add(asteroid);
    }

    IAsteroid CreateAsteroid(AsteroidType type = AsteroidType.LARGE)
    {
        //Create asteroids for the first time and use it as object pooling
        //Add the large asteroids to collection and add the sub asteroids(small and medium) as reference to its parent asteroid

        GameObject spawnedAsteroid = Instantiate(prefabMap[type]);
        spawnedAsteroid.SetActive(false);
        IAsteroid asteroid = spawnedAsteroid.GetComponent<IAsteroid>();
        OnResetCall += () => { ResetAsteroid(asteroid); };
        asteroid.RegisterDestroyedCallback(OnAsteroidDestroyed);


        switch (type)
        {
            case AsteroidType.LARGE:

                asteroid.TotalSubParticles = _gameManager.GameConfig.MediumSubParticles;
                for (var i = 0; i < asteroid.TotalSubParticles; i++)
                {
                    IAsteroid subAsteroid = CreateAsteroid(AsteroidType.MEDIUM);
                    asteroid.AddSubAsteroids(subAsteroid);
                }
                AddToCollection(asteroid);
                break;

            case AsteroidType.MEDIUM:
                asteroid.TotalSubParticles = _gameManager.GameConfig.SmallSubParticles;
                for (var i = 0; i < asteroid.TotalSubParticles; i++)
                {
                    IAsteroid subAsteroid = CreateAsteroid(AsteroidType.SMALL);
                    asteroid.AddSubAsteroids(subAsteroid);
                }
                break;
        }
        return asteroid;
    }




    IEnumerator SetupLargeAsteroids()
    {
        //Logic to spawn asteroids in an interval of time

        if (isSpawingAsteroids)
            yield break;


        isSpawingAsteroids = true;

        while (_gameManager.IsGameStarted && asteroidCollection.Any(x => !x.IsMarked))
        {
            yield return SpawnLargeAsteroid();
            yield return wait;
        }
        isSpawingAsteroids = false;
    }


    IEnumerator SpawnLargeAsteroid()
    {
        //Used to spawn the Large asteroids in the scene
        //Marking is done to make sure that immedietly destroyed objects are not spawned

        foreach (IAsteroid largeAsteroid in asteroidCollection)
        {
            //Debug.Log("IsMarked : " + largeAsteroid.IsMarked + " : IsActive : " +largeAsteroid.IsActive) ;
            if (!largeAsteroid.IsActive && !largeAsteroid.IsMarked)
            {
                Vector3 pos = PositionHelper.GetSpawnningPosition(PositionHelper.RandomEnumValue<SpawnSide>());
                Vector2 dir = (_playerManager.PlayerShip.Transform.position - pos).normalized;
                largeAsteroid.Mark();
                largeAsteroid.Initialize(pos, dir);

                break;
            }
        }
        yield return null;
    }

    private void OnAsteroidDestroyed(IAsteroid asteroid)
    {
        // Handles the score updation and check for when to spawn large asteroids


        _gameManager.UpdateScore(asteroid.Config.scoreValue);

        ShowParticleFX(asteroid.Transform.position);

        if (asteroid.Config.asteroidType == AsteroidType.SMALL && IsLargeAsteroidReadyToSpawn() &&  _gameManager.IsGameStarted)
        {
            UnMarkLargeAsteroids();
            if (asteroidCollection.Count() <= _gameManager.GameConfig.MaxAllowedLargeAsteroids)
            {
                //create new asteroids when one large asteroids to create difficulty
                CreateAsteroid();
            }
            StartCoroutine(SetupLargeAsteroids());
        }

        AudioEventHandler.Instance.PlaySFX(asteroid.Config.sfx);

    }

    private void UnMarkLargeAsteroids()
    {
        foreach (IAsteroid asteroid in asteroidCollection)
        {
            if (asteroid.AreAllChildParticleInactive())
            {
                asteroid.UnMark();
            }
        }
    }

    private void ShowParticleFX(Vector3 pos)
    {
        if (particleSystemPrefab != null)
        {
            Instantiate(particleSystemPrefab, pos, Quaternion.identity);
        }
    }

   
    /// <summary>
    /// Returns true if large asteroids are ready to spawn
    /// </summary>
    /// <returns></returns>
    private bool IsLargeAsteroidReadyToSpawn()
    {

        if (asteroidCollection.Count == 0)
            return false;

        return IsAnyLargeAstroidWithoutActiveChild();
    }

    /// <summary>
    /// Returns true if any large asteroids available to spawn
    /// </summary>
    /// <returns></returns>
    bool IsAnyLargeAstroidWithoutActiveChild()
    {
       return asteroidCollection.Any(x => x.AreAllChildParticleInactive());
    }
}

public interface IAsteroidController : IBaseGameComponent
{
    void SpawnEnemies();
}