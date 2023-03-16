using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUpConfig", menuName = "GameConfiguration/PowerUpConfig")]
public class PowerUpConfig : ScriptableObject
{
    [Header("Power up Config")]
    public PowerUpInfo[] powerUpInfos;
    public float powerUpSpeed;
    public float occuranceDelay;
}
