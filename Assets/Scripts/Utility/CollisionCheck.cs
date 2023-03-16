using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Utility for comparing layers
/// </summary>
public class CollisionCheck :MonoBehaviour
{
    [SerializeField, Layer]
    int asteroidLayer;
    [SerializeField, Layer]
    int wallLayer;
    [SerializeField, Layer]
    int playerLayer;
    [SerializeField, Layer]
    int bulletLayer;
    [SerializeField, Layer]
    int powerUpLayer;
    [SerializeField, Layer]
    int shieldLayer;

    static int ASTEROIDLAYER;
    static int WALLLAYER;
    static int PLAYERLAYER;
    static int BULLETLAYER;
    static int POWERUPLAYER;
    static int SHIELDLAYER;

    private void Start()
    {
        ASTEROIDLAYER = asteroidLayer;
        WALLLAYER = wallLayer;
        PLAYERLAYER = playerLayer;
        BULLETLAYER = bulletLayer;
        POWERUPLAYER = powerUpLayer;
        SHIELDLAYER = shieldLayer;
    }

    public static bool IsCollidedWithLayer(LayerMask mask , int self)
    {
        return ((mask.value & (1 << self)) > 0);
    }
    public static bool IsCollidedWithSelf(int selfLayer_id, int layer_id)
    {
        return layer_id == selfLayer_id;
    }

    public static bool IsCollidedWithAsteroid(int layer_id)
    {
        return layer_id == ASTEROIDLAYER;
    }
    public static bool IsCollidedWithBullet(int layer_id)
    {
        return layer_id == BULLETLAYER;
    }
    public static bool IsCollidedWithWall(int layer_id)
    {
        return layer_id == WALLLAYER;
    }
    public static bool IsCollidedWithPlayer(int layer_id)
    {
        return layer_id == PLAYERLAYER;
    }
    public static bool IsCollidedWithPowerUps(int layer_id)
    {
        return layer_id == POWERUPLAYER;
    }
    public static bool IsCollidedWithShield(int layer_id)
    {
        return layer_id == SHIELDLAYER;
    }
}
