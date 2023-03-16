using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public enum AsteroidType
{
    SMALL = 0,
    MEDIUM,
    LARGE
}

/// <summary>
/// Holds the config data for android
/// </summary>
[CreateAssetMenu(fileName = "NewAsteroidConfig", menuName = "GameConfiguration/AsteroidConfig")]
public class AsteroidConfig : ScriptableObject
{
    public int scoreValue;
    public float MinSpeed;
    public float MaxSpeed;
    public Sprite[] textures;
    public bool IsBreakable;
    public AsteroidType asteroidType;
    [Audio(AudioType.SFX)]
    public int sfx;

    public Sprite GetTexture()
    {
        return textures[Random.Range(0, textures.Length)];
    }

}
