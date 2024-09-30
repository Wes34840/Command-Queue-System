using System;
using UnityEngine;

public class BoundaryHandler : MonoBehaviour
{

    public static Transform[] tilePath;

    private void Start()
    {
        GameObject tileHolder = GameObject.Find("TileHolder");
        tilePath = new Transform[tileHolder.transform.childCount];
        for (int i = 0; i < tileHolder.transform.childCount; i++)
        {
            tilePath[i] = tileHolder.transform.GetChild(i);
        }
    }

    public static void EndOnWinTile()
    {
        GameMode.onWinGame.Invoke();
    }

    public static void OutOfBounds()
    {
        GameMode.onLoseGame.Invoke(true);
    }

    public static void EndOnNormalTile()
    {
        GameMode.onLoseGame.Invoke(false);
    }

    public static bool IsPlayerPositionValid(Vector3 playerPos)
    {
        // If a tile with the same position as the player doesn't exist
        if (!Array.Exists(tilePath, t => t.position == playerPos))
        {
            return false;
        }
        return true;
    }

}
