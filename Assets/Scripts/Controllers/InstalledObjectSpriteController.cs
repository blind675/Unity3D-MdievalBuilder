using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObjectSpriteController : MonoBehaviour
{
    Dictionary<InstalledObject, GameObject> installedObjectGameObjectsMap;

    Dictionary<string, Sprite> installedObjectSpritesMap;

    World world {
      get { return WorldController.Instance.World; }
    }
    // Start is called before the first frame update
    void Start()
    {

      installedObjectSpritesMap = new Dictionary<string, Sprite>();
      Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Tiles");

      foreach(Sprite sprite in sprites)
      {
          installedObjectSpritesMap[sprite.name] = sprite;
      }

      world.RegisterInstalledObjectCreatedCallback(OnInstalledObjectCreated);
        
      installedObjectGameObjectsMap = new Dictionary<InstalledObject, GameObject>();

    }

    public void OnInstalledObjectCreated(InstalledObject obj)
    {
        GameObject obj_go = new GameObject();

        installedObjectGameObjectsMap.Add(obj, obj_go);

        obj_go.name = obj.objectType + "_" + obj.tile.x + "_" + obj.tile.y;
        obj_go.transform.position = new Vector3(obj.tile.x, obj.tile.y, 0);
        obj_go.transform.SetParent(this.transform);

        SpriteRenderer renderer = obj_go.AddComponent<SpriteRenderer>();
        renderer.sprite = GetSpriteForInstalledObject(obj);
        renderer.sortingLayerName = "InstalledObjects";
        //renderer.sortingOrder = 1;

        obj.RegisterOnChangedCallback(OnInstalledObjectChanged);

    }

    public Sprite GetSpriteForInstalledObject(InstalledObject obj)
    {
      if(obj.linksToNeighbour == false) {
        return installedObjectSpritesMap[obj.objectType];
      } 

      string spriteName = obj.objectType + "_";

      Tile t;
      
      t = world.GetTileAt(obj.tile.x, obj.tile.y+1);
      if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
        spriteName += "N";
      }

      t = world.GetTileAt(obj.tile.x+1, obj.tile.y);
      if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
        spriteName += "E";
      }
      t = world.GetTileAt(obj.tile.x, obj.tile.y-1);
      if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
        spriteName += "S";
      }
      t = world.GetTileAt(obj.tile.x-1, obj.tile.y);
      if(t!= null && t.installedObject != null && t.installedObject.objectType == obj.objectType) {
        spriteName += "W";
      }

      if(installedObjectSpritesMap.ContainsKey(spriteName) == false) {
        Debug.LogError("No sprite for: " + spriteName);
        return null;
      }

      return installedObjectSpritesMap[spriteName];
    }

    public Sprite GetSpriteForInstalledObject(string objectType)
    {
      if(installedObjectSpritesMap.ContainsKey(objectType)) {
        return installedObjectSpritesMap[objectType];
      }

      if(installedObjectSpritesMap.ContainsKey(objectType+ "_")) {
        return installedObjectSpritesMap[objectType+ "_"];
      }

      return null;
    }
  

    void OnInstalledObjectChanged(InstalledObject obj)
    {
      // installedObjectGameObjectsMap[obj].GetComponent<SpriteRenderer>().sprite = wallSprite;
      if(installedObjectGameObjectsMap.ContainsKey(obj) == false) {
        Debug.LogError("InstalledObject "+obj.objectType+" not found in installedObjectGameObjectsMap");
        return;
      }

      GameObject installedObjectGameObject = installedObjectGameObjectsMap[obj];
      installedObjectGameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteForInstalledObject(obj);
    }
}
