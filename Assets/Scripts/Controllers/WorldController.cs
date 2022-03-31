using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    public static WorldController Instance { get; protected set; }

    public World World { get; protected set; }
 
    // Start is called before the first frame update
    void OnEnable()
    {
        if(Instance != null)
        {
            Debug.LogError("There should never be two world controllers.");
        }
        Instance = this;
        
        World = new World();
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }
}
