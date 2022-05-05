using UnityEngine;
using System.Collections.Generic;
using System;

public class World
{

    Tile[,] tiles;
    List<Character> characters;

    Dictionary<string, InstalledObject> installedObjectsPrototypes;

    public int width { get; }
    public int height { get; }

    Action<InstalledObject> cbInstalledObjectCreated;
    Action<Tile> cbTileChanged;
    Action<Character> cbCharacterCreated;

    public JobQueue jobQueue;

    public World(int width = 200, int height = 200)
    {
        this.width = width;
        this.height = height;

        tiles = new Tile[width, height];
        jobQueue = new JobQueue();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
                tiles[x, y].RegisterTileChangedCallback(OnTileChanged);
            }
        }

        Debug.Log("World created with " + (width * height) + " tiles");

        CreateInstalledObjectPrototypes();

        characters = new List<Character>();
        
    }

    public void CreateCharacter(Tile t) {
        Character c = new Character(t);
        characters.Add(c);

        if (cbCharacterCreated != null) {
            cbCharacterCreated(c);
        }
    }

    void CreateInstalledObjectPrototypes()
    {
        installedObjectsPrototypes = new Dictionary<string, InstalledObject>();

        installedObjectsPrototypes.Add("Wall", InstalledObject.CreatePrototype("Wall", 0f, 1, 1, true));

    }

    // public void RandomizeTiles() {
    //   Debug.Log("World randomized tiles");

    //   for (int x = 0; x < width; x++) {
    //     for (int y = 0; y < height; y++) {

    //       if(UnityEngine.Random.Range(0,2) == 0) {
    //         tiles[x, y].type = TileType.Dirt;
    //       } else {
    //         tiles[x, y].type = TileType.Empty;
    //       }
    //     }
    //   }
    // }

    public Tile GetTileAt(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }
        return tiles[x, y];
    }

    public void PlaceInstalledObject(string objectType, Tile tile)
    {
        if (installedObjectsPrototypes.ContainsKey(objectType) == false)
        {
            Debug.LogError("installedObjectsPrototypes does not contain key: " + objectType);
            return;
        }

        InstalledObject obj = InstalledObject.PlaceInstance(installedObjectsPrototypes[objectType], tile);

        if (obj == null)
        {
            Debug.LogError("Could not place object of type " + objectType + " on tile " + tile.x + "," + tile.y);
            return;
        }

        if (cbInstalledObjectCreated != null)
        {
            cbInstalledObjectCreated(obj);
            tile.pendingJob = null;
        }
    }

    public void RegisterCharacterCreatedCallback(Action<Character> callback)
    {
        cbCharacterCreated += callback;
    }

    public void UnregisterCharacterCreatedCallback(Action<Character> callback)
    {
        cbCharacterCreated -= callback;
    }

    public void RegisterInstalledObjectCreatedCallback(Action<InstalledObject> callback)
    {
        cbInstalledObjectCreated += callback;
    }

    public void UnregisterInstalledObjectCreatedCallback(Action<InstalledObject> callback)
    {
        cbInstalledObjectCreated -= callback;
    }

    public void RegisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged += callback;
    }

    public void UnregisterTileChangedCallback(Action<Tile> callback)
    {
        cbTileChanged -= callback;
    }

    void OnTileChanged(Tile tile)
    {
        if (cbTileChanged != null)
        {
            cbTileChanged(tile);
        }
    }

    public bool IsInstalledObjectPlacementValid(string installedObjectType, Tile tile)
    {
        if (installedObjectsPrototypes.ContainsKey(installedObjectType) == false)
        {
            Debug.LogError("installedObjectsPrototypes does not contain key: " + installedObjectType);
            return false;
        }

        return installedObjectsPrototypes[installedObjectType].IsValidePosition(tile); ;
    }

    public InstalledObject GetInstalledObjectPrototype(string objectType)
    {
        if (installedObjectsPrototypes.ContainsKey(objectType) == false)
        {
            Debug.LogError("installedObjectsPrototypes does not contain key: " + objectType);
            return null;
        }

        return installedObjectsPrototypes[objectType];
    }
}
