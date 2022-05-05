using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
   public float x {
        get {
            return Mathf.Lerp(currTile.x, destTile.x, movementPercentage);
        }
   }

    public float y
    {
        get
        {
            return Mathf.Lerp(currTile.y, destTile.y, movementPercentage);
        }
    }

    public Tile currTile;
    Tile destTile;
    float movementPercentage;

    float speed = 2f; // Tiles per second

    public Character(Tile tile) {
        currTile = tile;
    }

    public void Update(float deltaTime) {
        if (currTile == destTile) {
            return;
        }

        float distanceToTravel = Mathf.Sqrt(Mathf.Pow(currTile.x - destTile.x, 2) + Mathf.Pow(currTile.y - destTile.y, 2));

        float distanceThisFrame = speed * deltaTime;

        float percentageThisFrame = distanceToTravel / distanceThisFrame;

        movementPercentage += percentageThisFrame;

        if (movementPercentage >= 1) {
            // reached the destination
            currTile = destTile;
            movementPercentage = 0;
        }
    }

    public void SetDestination(Tile tile) {
        if (currTile.IsNeighbour(tile, true) == false) {
            Debug.LogError("Character::SetDestination - not neighour tile");
        }

        destTile = tile;
    }
}
