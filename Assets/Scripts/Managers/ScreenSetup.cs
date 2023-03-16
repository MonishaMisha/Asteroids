using System;
using UnityEngine;

/// <summary>
/// Creates a border around screen
/// </summary>
public class ScreenSetup : MonoBehaviour
{
    public static event Action OnScreenUpdate;

    public static Vector2 leftBottomCorner;
    public static Vector2 rightTopCorner;

    Camera mainCamera;
    // Start is called before the first frame update
    [SerializeField]
    float colliderThickness;
    [SerializeField]
    float  Posoffset;

    public static float offsetPosition ;
    float lengthoffset ;

    BoxCollider2D upperEdge;
    BoxCollider2D lowerEdge;
    BoxCollider2D leftEdge;
    BoxCollider2D rightEdge;


    Vector2 screenRes;


    void Start()
    {
        mainCamera = Camera.main;

        offsetPosition = (colliderThickness * 0.5f) + Posoffset;
        lengthoffset = colliderThickness * 2f + (Posoffset * 2f);

        GenerateBoxCollidersAcrossScreen();
        screenRes = new Vector2(Screen.width, Screen.height);
    }


    // Update is called once per frame
    void Update()
    {
        if (screenRes.x != Screen.width || screenRes.y != Screen.height)
        {
            UpdateView();

            OnScreenUpdate?.Invoke();

            UpdateColliders();
            screenRes.x = Screen.width;
            screenRes.y = Screen.height;
        }
    }
    void GenerateBoxCollidersAcrossScreen()
    {
        //generate 4 walls on all the sides of screen

        int layer = LayerMask.NameToLayer("Wall");

        upperEdge = new GameObject("upperEdge").AddComponent<BoxCollider2D>();
        upperEdge.gameObject.layer = layer;

        lowerEdge = new GameObject("lowerEdge").AddComponent<BoxCollider2D>();
        lowerEdge.gameObject.layer = layer;

        leftEdge = new GameObject("leftEdge").AddComponent<BoxCollider2D>();
        leftEdge.gameObject.layer = layer;

        rightEdge = new GameObject("rightEdge").AddComponent<BoxCollider2D>();
        rightEdge.gameObject.layer = layer;
        UpdateView();
        UpdateColliders();
    }

    void UpdateColliders()
    {
        //Updates the width height and position with respect to the viewport

        upperEdge.transform.position = new Vector3(0, rightTopCorner.y + offsetPosition, 0);
        upperEdge.size = new Vector2((rightTopCorner.x * 2f) + lengthoffset, colliderThickness);

        lowerEdge.transform.position = new Vector3(0, leftBottomCorner.y - offsetPosition, 0);
        lowerEdge.size = new Vector2((rightTopCorner.x * 2f) + lengthoffset, colliderThickness);

        leftEdge.transform.position = new Vector3(leftBottomCorner.x - offsetPosition, 0, 0);
        leftEdge.size = new Vector2(colliderThickness, (rightTopCorner.y * 2f) + lengthoffset);

        rightEdge.transform.position = new Vector3(rightTopCorner.x + offsetPosition, 0, 0);
        rightEdge.size = new Vector2(colliderThickness, (rightTopCorner.y * 2f) + lengthoffset);
    }

    /// <summary>
    /// Gets the left bottom corner world coordinates of the screen 
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetLeftBottomWallCorners()
    {
        return new Vector2(leftBottomCorner.x - offsetPosition, leftBottomCorner.y - offsetPosition);
    }

    /// <summary>
    /// Gets the right top corner world coordinates of the screen 
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetRightTopWallCorners()
    {
        return new Vector2(rightTopCorner.x + offsetPosition, rightTopCorner.y + offsetPosition);
    }

    void UpdateView()
    {
        // Gets the world space of topright and bottomleft corner of screen for reference to the screen size

        leftBottomCorner = mainCamera.ViewportToWorldPoint(new Vector3(0, 0f, mainCamera.nearClipPlane));
        rightTopCorner = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));
    }
    /// <summary>
    /// Get a random world point between bottom and top of screen
    /// </summary>
    /// <returns></returns>
    public static float GetRandomPointScreenHeight()
    {
        return UnityEngine.Random.Range(leftBottomCorner.y, rightTopCorner.y);
    }
    /// <summary>
    /// Get a random world point between left and right side of screen
    /// </summary>
    /// <returns></returns>
    public static float GetRandomPointScreenWidth()
    {
        return UnityEngine.Random.Range(leftBottomCorner.x, rightTopCorner.x);
    }
    /// <summary>
    /// Get a random world point between bottom and top of screen
    /// </summary>
    /// <returns></returns>
    public static float GetRandomPointScreenHeight(float padding)
    {
        float leftBottCornerY = leftBottomCorner.y + padding;
        float rightTopCornerY = rightTopCorner.y - padding;
        return UnityEngine.Random.Range(leftBottCornerY, rightTopCornerY);
    }
    /// <summary>
    /// Get a random world point between left and right side of screen
    /// </summary>
    /// <returns></returns>
    public static float GetRandomPointScreenWidth(float padding)
    {
        float leftBottCornerX = leftBottomCorner.x + padding;
        float rightTopCornerX = rightTopCorner.x - padding;
        return UnityEngine.Random.Range(leftBottCornerX, rightTopCornerX);
    }

}
