using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all the properties and does the funtionality of a Player(Ship)
/// </summary>
public class PlayerShip : MovableObject, IPlayerShip
{
    [SerializeField]
    private PlayerConfig config;
    [SerializeField]
    private ParticleSystem thrustFX, deathFX;
    [SerializeField]
    private GameObject shield;


    public PlayerConfig Config
    {
        get => config;
    }

    [SerializeField]
    LayerMask damagableLayer;

    private int health;
    public int Health
    {

        get => health;
    }
    public float MaxSpeed { get; set; }

    private bool isAlive;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }

    private bool hasShield;
    public bool HasShield
    {
        get
        {
            return hasShield;
        }
    }

    public LayerMask DamagableLayer { get => damagableLayer; }

    private IPlayerManager _playerManager;

    bool inputAccelerate;

    private void OnEnable()
    {
        InputController.OnThrustInputEvent += OnThrustInputEventReceived;
    }

    private void OnDisable()
    {
        InputController.OnThrustInputEvent -= OnThrustInputEventReceived;
      
        if (thrustFX.isPlaying)
        {
            thrustFX.Stop();
        }
        AudioEventHandler.Instance?.PlayEngineSound(false);
    }

    private void OnThrustInputEventReceived(bool isPerformed)
    {
        //Plays a sound when engine revs

        inputAccelerate = isPerformed;
        AudioEventHandler.Instance.PlayEngineSound(inputAccelerate);
    }

    /// <summary>
    /// Used to Accelerate the ship
    /// </summary>
    public void Accelarate()
    {

        speed = Mathf.MoveTowards(speed, MaxSpeed, Time.deltaTime * config.moveSentivity);
        speed = Mathf.Clamp(speed, 0, MaxSpeed);
        direction = Vector2.Lerp(direction, transform.up.normalized, Time.deltaTime * config.breakingSernsitivity);

        // Add an particleFX when accelarated
        if (thrustFX.isStopped)
        {
            thrustFX.Play();
        }

    }

    /// <summary>
    /// Used to apply rotational torque to ship
    /// </summary>
    /// <param name="torque"></param>
    public void ApplyTorque(float torque)
    {
        transform.Rotate(Vector3.forward * torque, Space.World);
    }

    /// <summary>
    /// Handles the damage to the ship
    /// </summary>
    public void OnDamage()
    {
        if (hasShield)
        {
            BreakShield();
            return;
        }

        isAlive = false;
        health--;

        if (deathFX != null)
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
        }

        SetActive(false);
        _playerManager.PlayerDestroyed();
    }

    /// <summary>
    /// Reset the player for a new session
    /// </summary>
    /// <param name="isNewgame"></param>
    public void ResetPlayer(bool isNewgame = false)
    {
        isAlive = true;
        speed = 0;
        direction = Vector2.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        inputAccelerate = false;

        if (isNewgame)
        {
            OnInitialize();
        }

        SetActive(true);
    }

    private new void Update()
    {
        //logic to apply torque and drag

        if (!isAlive)
            return;


        if (inputAccelerate)
        {
            Accelarate();
        }
        else
        {
            if (thrustFX.isPlaying)
            {
                thrustFX.Stop();
            }
            speed = Mathf.MoveTowards(speed, 0f, Time.deltaTime * config.breakingSernsitivity);
        }
        ApplyTorque(InputController.SteerSensitivity * config.steerSensitivity);



        base.OnMove();

    }

    public void SetActiveShield(bool active)
    {
        shield.SetActive(active);
        hasShield = active;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
      
            
        if (CollisionCheck.IsCollidedWithLayer(damagableLayer, collision.collider.gameObject.layer))
        {

            if (CollisionCheck.IsCollidedWithBullet(collision.collider.gameObject.layer))
            {
                IBullet bullet = collision.collider.GetComponent<IBullet>();
                if (bullet != null)
                {
                    if (!bullet.CanDamagePlayer)
                        return;

                    bullet.HitTarget();
                }
            }
            OnDamage();
        }
    }

    private void BreakShield()
    {
        SetActiveShield(false);
    }

    public void OnInitialize()
    {
        isAlive = true;
        MaxSpeed = config.MaxSpeed;
        health = config.TotalLife;
        SetActiveShield(false);

    }
    protected override void Start()
    {
        base.Start();

        if (config == null)
        {
            throw new NullReferenceException("Player Config is missing");
        }
    }

    public void AddManagerDependency(IPlayerManager manager)
    {
        _playerManager = manager;
    }
}
public interface IPlayerShip : IDamagable, IMovable
{
    void OnInitialize();
    void AddManagerDependency(IPlayerManager manager);
    PlayerConfig Config { get; }
    bool IsAlive { get; }
    float MaxSpeed { get; set; }
    bool HasShield { get; }
    void Accelarate();
    void SetActiveShield(bool active);
    void ResetPlayer(bool isNewgame = false);
}