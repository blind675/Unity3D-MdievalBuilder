using System;
using UnityEngine;

public enum TileType { Empty, Grass, Dirt, Sand, Rock, Water };


public class Tile
{

    Action<Tile> cbTileChanged;

    TileType _type = TileType.Grass;
    public TileType type
    {
        get
        {
            return _type;
        }
        set
        {
            TileType oldType = _type;
            _type = value;

            if (cbTileChanged != null && oldType != value)
            {
                cbTileChanged(this);
            }
        }
    }

    public InstalledObject installedObject { get; protected set; }

    public Job pendingJob;

    public World world { get; }
    public int x { get; }
    public int y { get; }

    public Tile(World world, int x, int y)
    {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    public void RegisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

    public bool PlaceInstalledObject(InstalledObject obj)
    {
        if (installedObject != null || obj == null)
        {
            Debug.LogError("Cannot place object of type " + obj.objectType + " on tile ");
            return false;
        }

        installedObject = obj;
        return true;
    }

    public bool IsNeighbour(Tile tile, bool diagonalsCheck = false)
    {
        if (this.x == tile.x && (this.y == tile.y + 1 || this.y == tile.y - 1))
        {
            return true;
        }

        if (this.y == tile.y && (this.x == tile.x + 1 || this.x == tile.x - 1))
        {
            return true;
        }

        if (diagonalsCheck)
        {
            if (this.x == tile.x + 1 && (this.y == tile.y + 1 || this.y == tile.y - 1))
            {
                return true;
            }

            if (this.x == tile.x - 1 && (this.y == tile.y + 1 || this.y == tile.y - 1))
            {
                return true;
            }
        }
        return false;
    }
}
