using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public enum SpawnSide
{
    right,
    left,
    top,
    bottom
}

public static class PositionHelper
{
    public static Vector2 GetSpawnningPosition(SpawnSide side)
    {
        Vector2 position = new Vector2();
        switch (side)
        {
            case SpawnSide.right:

                position.x = ScreenSetup.rightTopCorner.x + 2f;
                position.y = ScreenSetup.GetRandomPointScreenHeight();
                break;

            case SpawnSide.left:
                position.x = ScreenSetup.leftBottomCorner.x - 2f;
                position.y = ScreenSetup.GetRandomPointScreenHeight();
                break;

            case SpawnSide.top:
                position.x = ScreenSetup.GetRandomPointScreenWidth();
                position.y = ScreenSetup.rightTopCorner.y + 2f;
                break;

            case SpawnSide.bottom:
                position.x = ScreenSetup.GetRandomPointScreenWidth();
                position.y = ScreenSetup.leftBottomCorner.y - 2f;
                break;

        }

        return position;
    }


    /// <summary>
    /// Returns true if a point is inside the collider walls
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static bool IsInsideWall(Vector2 pos)
    {
        var leftBottomWallCorner = ScreenSetup.GetLeftBottomWallCorners();
        var rightTopWallCorner = ScreenSetup.GetRightTopWallCorners();
        return (pos.x > leftBottomWallCorner.x && pos.x < rightTopWallCorner.x && pos.y > leftBottomWallCorner.y && pos.y < rightTopWallCorner.y) ;
    }

    /// <summary>
    /// Get random position inside screen
    /// </summary>
    /// <param name="paddingX"></param>
    /// <param name="paddingY"></param>
    /// <returns></returns>
    public static Vector2 GetRandomSpawningPointInsideScreen(float paddingX = 0, float paddingY = 0)
    {
        Vector2 position = new Vector2();
        position.x = ScreenSetup.GetRandomPointScreenWidth(paddingX);
        position.y = ScreenSetup.GetRandomPointScreenHeight(paddingY);
        return position;
    }

    public static Random _R = new Random();
    public static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(_R.Next(v.Length));
    }
}
