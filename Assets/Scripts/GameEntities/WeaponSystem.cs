using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Handles the weapon system of ship
/// </summary>
[RequireComponent(typeof(IPlayerShip))]
public class WeaponSystem : MonoBehaviour
{

    bool isFiring;

    public bool IsFiring { get => isFiring; }

    [SerializeField]
    private GameObject ammoPrefab;

    IPlayerShip playerShip;

    Queue<IBullet> availableBullets = new Queue<IBullet>();

    BulletInfo[] bulletVarients;

    WaitForSeconds delay;

    private Action OnResetCall;

    bool canFire = true;

    bool burstFireEnabled;
    int burstFireCapacity;
    BulletType currentType = BulletType.Normal;

    int currentBurstFireCount;

    private void OnEnable()
    {
        InputController.OnFireInputEvent += OnFireInputEvent;
        PlayerManager.OnPowerUpAquired += OnPowerUpAquired;
        PlayerManager.OnPowerUpLost += OnPowerUpLost;
    }

    /// <summary>
    /// Handles power up lost 
    /// </summary>
    /// <param name="powerUpType"></param>
    private void OnPowerUpLost(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.CresentShot:
                SetUpDefaultShots();
                break;
            case PowerUpType.Shield:
                DisableShield();
                break;
        }
        }

    private void DisableShield()
    {
        playerShip.SetActiveShield(false);
    }

    private void SetUpDefaultShots()
    {
        currentType = BulletType.Normal;
    }

    /// <summary>
    /// Set the powerup to weapons
    /// </summary>
    /// <param name="powerUpType"></param>
    private void OnPowerUpAquired(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.CresentShot:
                SetUpCresentShots();
                break;
            case PowerUpType.Shield:
                SetUpShield();
                break;
        }
    }

    private void SetUpShield()
    {
        playerShip.SetActiveShield(true);
    }

    private void SetUpCresentShots()
    {
        currentType = BulletType.Cresent;
    }

    private void OnFireInputEvent(bool isPerformed)
    {
        //Listen to input and fire when performed
        if (isPerformed)
        {
            StartFire();
        }
        else
        {
            StopFire();
        }
    }

    private void OnDisable()
    {
        InputController.OnFireInputEvent -= OnFireInputEvent;
        PlayerManager.OnPowerUpAquired -= OnPowerUpAquired;
        PlayerManager.OnPowerUpLost -= OnPowerUpAquired;
    }
    private void OnGameReset()
    {
        StopFire();
        canFire = true;

        availableBullets.Clear();
        currentType = BulletType.Normal;
        OnResetCall?.Invoke();
    }
    private void OnDestroy()
    {
        GameManager.OnGameReset -= OnGameReset;
    }

    void Start()
    {
        playerShip = GetComponent<IPlayerShip>();
        delay = new WaitForSeconds(1f / playerShip.Config.FireringRate);
        burstFireEnabled = playerShip.Config.burstFire;
        burstFireCapacity = playerShip.Config.burstFireCapacity;
        bulletVarients = playerShip.Config.bulletsVarients;
        FillUpMagazine();

        GameManager.OnGameReset += OnGameReset;
    }
    void ResetBullet(IBullet bullet)
    {
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }
    private void FillUpMagazine()
    {
        //Create all the bullets and keep it in a collection

        for (int i = 0; i < playerShip.Config.TotalBullets; i++)
        {
            GameObject bulletObject = Instantiate(ammoPrefab);
            IBullet bullet = bulletObject.GetComponent<IBullet>();
            bullet.RegisterDestroyedCallback(OnBulletDown);

            OnResetCall += () => { ResetBullet(bullet); };

            availableBullets.Enqueue(bullet);
            bulletObject.SetActive(false);
        }

    }

    void OnBulletDown(IBullet bullet)
    {
        bullet.SetActive(false);
        availableBullets.Enqueue(bullet);
    }

    /// <summary>
    /// Starts the firing
    /// </summary>
    public void StartFire()
    {
        isFiring = true;
        StartCoroutine(ShootBullet());
    }

    IEnumerator ShootBullet()
    {

        if (burstFireEnabled)
        {
            while (currentBurstFireCount < burstFireCapacity)
            {
                yield return ActivateBurstFire();
                currentBurstFireCount++;
            }
            currentBurstFireCount = 0;
        }
        else
        {
            while (isFiring)
            {
                yield return ActivateAmmo();
            }
        }
    }
    IEnumerator ActivateBurstFire()
    {
        if (!canFire)
            yield break;

        if (availableBullets.Count > 0)
        {
            canFire = false;
            FireBullet();
            yield return delay;
        }
        canFire = true;
    }


    IEnumerator ActivateAmmo()
    {
        if (!canFire)
            yield break;

        if (availableBullets.Count > 0)
        {
            canFire = false;
            FireBullet();
            yield return delay;
        }
        canFire = true;
    }

    void FireBullet()
    {
        IBullet bullet = availableBullets.Dequeue();
        if (bullet.Type != currentType)
        {
            bullet.SetType(currentType, bulletVarients.FirstOrDefault(x => x.BulletType == currentType).sprite);
        }
        Vector2 direction = playerShip.Transform.up.normalized;
        float speed = (playerShip.Config.BulletSpeed + playerShip.Speed);

        bullet.Fire(direction, speed, playerShip.Transform.position, playerShip.Config.BulletLifespan);

        AudioEventHandler.Instance.PlaySFX(playerShip.Config.fireSFX);
    }


    public void StopFire()
    {
        isFiring = false;
    }



}

