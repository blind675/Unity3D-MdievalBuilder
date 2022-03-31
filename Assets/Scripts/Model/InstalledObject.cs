using System;
using UnityEngine;

public class InstalledObject
{
    Action<InstalledObject> cbOnChanged;
    Func<Tile, bool> funcPositionValidation;
    
    public Tile tile {get; protected set;}

    public string objectType {get; protected set;}

    float movementCost;

    int width;
    int height;

    public bool linksToNeighbour {get; protected set;} = false;

    protected InstalledObject() { }

    static public InstalledObject CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height = 1, bool linksToNeighbour = false) 
    {
      InstalledObject obj = new InstalledObject();
      obj.objectType = objectType;
      obj.movementCost = movementCost;
      obj.width = width;
      obj.height = height;
      obj.linksToNeighbour = linksToNeighbour;

      obj.funcPositionValidation = obj.__IsValidePosition;

      return obj;
    }

    static public InstalledObject PlaceInstance(InstalledObject prototype, Tile tile)
    {
        if(prototype.funcPositionValidation(tile) == false) {
            Debug.LogError("Cannot place object of type " + prototype.objectType + " on tile " + tile.x + "," + tile.y);
            return null;
        }

        InstalledObject obj = new InstalledObject();
        obj.tile = tile;
        obj.objectType = prototype.objectType;
        obj.movementCost = prototype.movementCost;
        obj.width = prototype.width;
        obj.height = prototype.height;
        obj.linksToNeighbour = prototype.linksToNeighbour;

        if(tile.PlaceInstalledObject(obj) == false) {
          return null;
        }

        if(obj.linksToNeighbour) {
          Tile t;
      
          t = tile.world.GetTileAt(tile.x, tile.y+1);
          if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
            if(t.installedObject.cbOnChanged != null) {
              t.installedObject.cbOnChanged(t.installedObject);
            }
          }

          t = tile.world.GetTileAt(tile.x+1, tile.y);
          if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
            if(t.installedObject.cbOnChanged != null) {
              t.installedObject.cbOnChanged(t.installedObject);
            }
          }
          t = tile.world.GetTileAt(tile.x, tile.y-1);
          if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
            if(t.installedObject.cbOnChanged != null) {
              t.installedObject.cbOnChanged(t.installedObject);
            }
          }
          t = tile.world.GetTileAt(tile.x-1, tile.y);
          if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
            if(t.installedObject.cbOnChanged != null) {
              t.installedObject.cbOnChanged(t.installedObject);
            }
          }
        }

        return obj;
    }

    public void RegisterOnChangedCallback(Action<InstalledObject> callback)
    {
        cbOnChanged += callback;
    }

    public void UnregisterOnChangedCallback(Action<InstalledObject> callback)
    {
        cbOnChanged -= callback;
    }

    public bool IsValidePosition(Tile tile) {
      return this.funcPositionValidation(tile);
    }

    public bool __IsValidePosition(Tile tile) {

      if(tile.type != TileType.Dirt) {
        return false;
      }

      if(tile.installedObject != null) {
        return false;
      }
      
      return true;
    }

    public bool __IsValidePosition_Door(Tile tile) {
      if(__IsValidePosition(tile) == false) {
        return false;
      }

      return true;
    }
}
