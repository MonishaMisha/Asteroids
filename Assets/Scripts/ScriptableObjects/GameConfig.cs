using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Holds the config data for game
/// </summary>
[CreateAssetMenu(fileName = "NewGameConfig", menuName = "GameConfiguration/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Asteroid")]
    public float AsteroidSpawningInterval;

    public int MaxAllowedLargeAsteroids;
    
    public int TotalLargeAsteroids;

    public int MediumSubParticles;

    public int SmallSubParticles;
}
