using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Power up entity
/// </summary>
public class PowerUp : MonoBehaviour, IPowerUp
{
    public Action<IPowerUp> OnDestroy { get; private set; }

    public PowerUpType PowerUpType { get; private set; }

    public float CoolDownTime { get; private set; }

    public Vector2 Size => spriteRenderer.size;

    private Action<IPowerUp> OnInteraction;
    [SerializeField]
    LayerMask interactionLayer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {

        int collidedLayer = collision.collider.gameObject.layer;
        if (CollisionCheck.IsCollidedWithLayer(interactionLayer, collidedLayer))
        {
            OnInteraction.Invoke(this);
        }

    }


    public void Initialize(Vector2 position)
    {
        transform.position = position;
        SetActive(true);
    }

    /// <summary>
    /// Registers On Interaction callbacks
    /// </summary>
    /// <param name="callback"></param>
    public void RegisterInteractionCallback(Action<IPowerUp> callback)
    {
        OnInteraction += callback;
    }

    /// <summary>
    /// Set the type
    /// </summary>
    /// <param name="type"></param>
    public void SetType(PowerUpType type)
    {
        PowerUpType = type;
    }

    /// <summary>
    /// Sets the cooldown time
    /// </summary>
    /// <param name="coolDownTime"></param>
    public void SetCoolDownTime(float coolDownTime)
    {
        CoolDownTime = coolDownTime;
    }

    /// <summary>
    /// set active gameobject
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}

public interface IPowerUp : IInteractable<IPowerUp>
{
    PowerUpType PowerUpType { get; }
    float CoolDownTime { get; }
    void SetActive(bool active);
    Vector2 Size { get; }
    void SetType(PowerUpType type);
    void SetCoolDownTime(float coolDownTime);
    void Initialize(Vector2 position);
}
