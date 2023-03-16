using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// Holds all the behavior of an asteroid
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Asteroid : MovableObject, IAsteroid
{
    [SerializeField]
    AsteroidConfig config;
    [SerializeField]
    LayerMask breakableLayer;

    public bool IsBreakable
    {

        get => config.IsBreakable;
    }

    public int TotalSubParticles
    {
        get;
        set;
    }


    public bool IsActive { get => gameObject.activeSelf; }


    public AsteroidConfig Config { get => config; }

    public LayerMask BreakableLayer { get => breakableLayer; }

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    private Action<IAsteroid> OnDestroy;

    public bool IsMarked { get; private set; }

    private List<IAsteroid> subParticles = new List<IAsteroid>();

    /// <summary>
    /// To identify if any of the child specific to this asteroid is active in the screen
    /// </summary>
    /// <returns></returns>
    public bool IsSubParticleActive()
    {
        return subParticles.Any(x => x.IsActive);
    }

    /// <summary>
    /// To identify if any of the child/sub child particles are active in the screen
    /// </summary>
    /// <returns></returns>
    public bool IsAnyChildParticleActive()
    {
        if (subParticles.Count == 0)
        {
            return IsActive;
        }
        return IsActive || subParticles.Any(x => x.IsAnyChildParticleActive());
    }

    public bool AreAllChildParticleInactive()
    {
        if (subParticles.Count == 0)
        {
            return !IsActive;
        }
        return !IsActive && !subParticles.All(x => x.AreAllChildParticleInactive());
    }


    public void OnBreakage()
    {
        gameObject.SetActive(false);
        SpawnSubParticles();
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
        Initialize();


    }


    private void Initialize()
    {
        if (config != null)
        {
            spriteRenderer.sprite = config.GetTexture();
            AddPolygonCollider2D(polygonCollider, spriteRenderer.sprite);
        }
        else
        {
            throw new NullReferenceException("Asteroid config is missing");
        }
        speed = UnityEngine.Random.Range(config.MinSpeed, config.MaxSpeed);
    }

    /// <summary>
    /// Used to prepare an asteroid before spawning
    /// </summary>
    public void Initialize(Vector2 position, Vector2 direction)
    {
        Initialize();

        transform.position = position;
        Direction = direction;

        SetActive(true);
    }

    void SpawnSubParticles()
    {
        //Create subparticles after breaking
        foreach (IAsteroid subAsteroid in subParticles)
        {
            Vector3 pos = transform.position;
            Vector2 dir = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
            subAsteroid.Initialize(pos, dir);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        //ignores the logic to appear on the other side of the screen when asteroid hit the wall for the first time.
        //Idenfify the collsion with bullet


        base.OnCollisionEnter2D(collision);


        int collidedLayer = collision.collider.gameObject.layer;
        if (CollisionCheck.IsCollidedWithLayer(breakableLayer, collidedLayer))
        {
            OnBreakage();
        }
        if (CollisionCheck.IsCollidedWithSelf(gameObject.layer, collidedLayer))
        {
            var collidedObj = collision.GetContact(0).normal;
            direction = Vector2.Reflect(direction, collidedObj);
        }

    }


    /// <summary>
    /// Registers OnDestroy callbacks
    /// </summary>
    /// <param name="callback"></param>
    public void RegisterDestroyedCallback(Action<IAsteroid> callback)
    {
        OnDestroy = callback;
    }

    /// <summary>
    /// Draws a polygon collider at runtime for the sprite 
    /// </summary>
    /// <param name="polygon"></param>
    /// <param name="sprite"></param>
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

    /// <summary>
    /// Used to add sub particles
    /// </summary>
    /// <param name="asteroid"></param>
    public void AddSubAsteroids(IAsteroid asteroid)
    {
        subParticles.Add(asteroid);
    }

    public void Mark()
    {
        IsMarked = true;
    }

    public void UnMark()
    {
        IsMarked = false;
    }
}
public interface IAsteroid : IBreakable, IMovable, IMarkable, IDestroyedCallback<IAsteroid>
{
    bool IsActive { get; }
    int TotalSubParticles { get; set; }
    AsteroidConfig Config { get; }
    void Initialize(Vector2 position, Vector2 direction);
    bool IsSubParticleActive();
    bool IsAnyChildParticleActive();
    void AddSubAsteroids(IAsteroid asteroid);
    bool AreAllChildParticleInactive();

}