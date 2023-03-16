using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletType
{
    Normal,
    Cresent
}
[Serializable]
public struct BulletInfo
{
    public BulletType BulletType;
    public Sprite sprite;
}
/// <summary>
/// Holds the config data for Player
/// </summary>
[CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "GameConfiguration/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("General Config")]
    public int TotalLife;
    public float MaxSpeed;
    [Audio(AudioType.SFX)]
    public int deathSFX;
    [Audio(AudioType.SFX)]
    public int gameOverSFX;

    [Header("Ship Config")]
    public int moveSentivity;
    public int steerSensitivity;
    public int breakingSernsitivity;

    [Header("Weapon Config")]
    public int FireringRate;
    public bool burstFire;
    public int burstFireCapacity;
    public float BulletSpeed;
    public int TotalBullets;
    public float BulletLifespan;
    public BulletInfo[] bulletsVarients;
    [Audio(AudioType.SFX)]
    public int fireSFX;

}
