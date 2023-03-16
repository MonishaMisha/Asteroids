using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds all the properties and does the funtionality of a Bullet
/// </summary>
public class Bullet : MovableObject, IBullet
{
    bool canDamagePlayer;
    [SerializeField]
    LayerMask hitLayer;

    BulletType type;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    public BulletType Type => type;

    public bool CanDamagePlayer
    {
        get => canDamagePlayer;
    }

    public Action<IBullet> OnDestroy { get; private set; }

    public LayerMask HitLayer { get => hitLayer; }

    public void HitTarget()
    {
        OnDestroy.Invoke(this);
    }
    public void OnLifeOver()
    {
        OnDestroy.Invoke(this);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignores the collsion with the player for first time
        // Trigger callback when collided with player or asteroids

        base.OnCollisionEnter2D(collision);
        int collidedLayer = collision.collider.gameObject.layer;

        if (CollisionCheck.IsCollidedWithWall(collidedLayer))
        {
            canDamagePlayer = true;
        }
        if (CollisionCheck.IsCollidedWithAsteroid(collidedLayer))
        {
            HitTarget();
        }

    }

    public void RegisterDestroyedCallback(Action<IBullet> action)
    {
        OnDestroy = action;
    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);
        if (!active)
        {
            StopAllCoroutines();
            canDamagePlayer = false;
        }
    }

    /// <summary>
    /// Used to fire a bullet in the desired direction
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    /// <param name="position"></param>
    /// <param name="lifeTime"></param>
    public void Fire(Vector3 direction, float force, Vector3 position, float lifeTime)
    {
        transform.position = position;

        this.direction = direction;

        Vector2 target = position + direction;

        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        this.speed = force;

        SetActive(true);
        StartCoroutine(Deactivate(lifeTime));


    }

    IEnumerator Deactivate(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        OnLifeOver();
    }

    public void AddPolygonCollider2D(PolygonCollider2D polygon, Sprite sprite)
    {

        int shapeCount = sprite.GetPhysicsShapeCount();
        polygon.pathCount = shapeCount;
        var points = new List<Vector2>(64);
        for (int i = 0; i < shapeCount; i++)
        {
            sprite.GetPhysicsShape(i, points);
            polygon.SetPath(i, points);
        }
    }

    public void SetType(BulletType type, Sprite sprite)
    {
        this.type = type;
        spriteRenderer.sprite = sprite;
        AddPolygonCollider2D(polygonCollider, sprite);
    }
    /*protected override void Update()
{
    base.Update();
    Debug.DrawRay(transform.position, transform.up * 50, Color.green);
} */

}

public interface IBullet : IMovable, IDestroyedCallback<IBullet>
{
    LayerMask HitLayer { get; }
    BulletType Type { get; }
    void SetType(BulletType type, Sprite sprite);
    bool CanDamagePlayer { get; }
    void Fire(Vector3 direction, float force, Vector3 position, float lifeTime);
    void HitTarget();
}