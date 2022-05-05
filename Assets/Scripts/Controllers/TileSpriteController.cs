using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : MonoBehaviour
{
    public Sprite dirtSprite1;
    public Sprite dirtSprite2;
    public Sprite grassSprite1;
    public Sprite grassSprite2;

    Dictionary<Tile, GameObject> tileGameObjectsMap;

    World world {
      get { return WorldController.Instance.World; }
    }
    // Start is called before the first frame update
    void Start()
    {
      world.RegisterTileChangedCallback(OnTileTypeChanged);
        
      tileGameObjectsMap = new Dictionary<Tile, GameObject>();
      
       // create a game object to represent each tile
        for (int x = 0; x < world.width; x++)
        {
            for (int y = 0; y < world.height; y++)
            {
                Tile tile_data = world.GetTileAt(x, y);
                GameObject tile_go = new GameObject();

                tileGameObjectsMap.Add(tile_data, tile_go);

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.x, tile_data.y, 0);
                tile_go.transform.SetParent(this.transform);

                SpriteRenderer renderer = tile_go.AddComponent<SpriteRenderer>();
                renderer.sprite = Random.Range(0,2) == 0 ? grassSprite1 : grassSprite2;
                renderer.sortingLayerName = "Tiles";
                //renderer.sortingOrder = 0;
            }
        }

      // center the camera
      Camera.main.transform.position = new Vector3(world.width / 2, world.height / 2, Camera.main.transform.position.z);
    }

    void OnTileTypeChanged(Tile tile_data) {
      
        Debug.Log("OnTileTypeChanged");

        if(tileGameObjectsMap.ContainsKey(tile_data) == false)
        {
            Debug.LogError("tileGameObjectsMap does not contain tile: " + tile_data);
            return;
        }

        GameObject tile_go = tileGameObjectsMap[tile_data];

        switch(tile_data.type) {
          case TileType.Dirt:
            tile_go.GetComponent<SpriteRenderer>().sprite = Random.Range(0,2) == 0 ? dirtSprite1 : dirtSprite2;
            // tile_go.GetComponent<SpriteRenderer>().sprite = dirtSprite1;
            break;
          case TileType.Grass:
            tile_go.GetComponent<SpriteRenderer>().sprite = Random.Range(0,2) == 0 ? grassSprite1 : grassSprite2;
            break;
          default:
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
            break;
        }
       
    }

}
