using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that has movement, and can pass from one side to another side of screen
/// </summary>
public class MovableObject : MonoBehaviour, IMovable
{

    [SerializeField]
    protected float speed;

    public float Speed
    {
        get => speed;
        set 
        { 
            speed = value; 
        }
    }
    protected Vector2 direction;

    public Vector2 Direction
    {
        get => direction;
        set
        {
            direction = value;
        }
    }
    public Transform Transform { get => gameObject.transform; }


    private Camera mainCamera;

    protected virtual void Start()
    {
      mainCamera = Camera.main;
    }

    public void RemoveObject()
    {
        Destroy(this.gameObject);
    }

    protected virtual void Update()
    {
        OnMove();
    }

    public virtual void OnMove()
    {
        transform.Translate(direction * speed * Time.deltaTime , Space.World);
    }

    private void CheckOutsideBoundaryNew(Collider2D collider, Vector3 contact)
    {
        // Check from which side the collsion contact happened with wall

        Vector2 position2D = mainCamera.WorldToScreenPoint(contact);
        Vector2 lDCorner = mainCamera.ViewportToWorldPoint(new Vector3(0, 0f, mainCamera.nearClipPlane));
        Vector2 rUCorner = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));
        if (IsOutsideLeftBorder(position2D))
        {
            transform.position = new Vector3(rUCorner.x - (collider.bounds.extents.x * 0.2f), transform.position.y, 0f);
        }
        if (IsOutsideRightBorder(position2D))
        {
            transform.position = new Vector3(lDCorner.x + (collider.bounds.extents.x * 0.2f), transform.position.y, 0f);
        }
        if (IsOutsideTopBorder(position2D))
        {
            transform.position = new Vector3(transform.position.x, lDCorner.y + (collider.bounds.extents.y * 0.2f), 0f);
        }
        if (IsOutsideBottomBorder(position2D))
        {
            transform.position = new Vector3(transform.position.x, rUCorner.y - (collider.bounds.extents.y * 0.2f), 0f);
        }
    }


    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (CollisionCheck.IsCollidedWithWall(collision.collider.gameObject.layer) && PositionHelper.IsInsideWall(transform.position))
        {
            CheckOutsideBoundaryNew(collision.otherCollider , collision.GetContact(0).point);
        }
      // Debug.Log("collided with" + collision.collider.name);
    }



    bool IsOutsideLeftBorder(Vector2 pos)
    {
        return ( pos.x < 0);
    }
    bool IsOutsideRightBorder(Vector2 pos)
    {
        return (pos.x > Screen.width);
    }
    bool IsOutsideTopBorder(Vector2 pos)
    {
        return (pos.y > Screen.height);
    }
    bool IsOutsideBottomBorder(Vector2 pos)
    {
        return (pos.y < 0);
    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}

public interface IMovable
{
    Transform Transform { get; }
    float Speed { get; set; }
    Vector2 Direction { get; set; }
    void OnMove();
    void SetActive(bool active);
    void RemoveObject();

}