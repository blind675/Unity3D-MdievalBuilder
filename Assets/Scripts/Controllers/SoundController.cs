using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    float soundCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        WorldController.Instance.World.RegisterInstalledObjectCreatedCallback(OnInstalledObjectChanged);
        WorldController.Instance.World.RegisterTileChangedCallback(OnTileChanged);
    }

    void Update()
    {
        soundCooldown -= Time.deltaTime;
    }

    void OnTileChanged(Tile tile_data) {
    
        if(soundCooldown > 0) {
            return;
        }

        switch(tile_data.type) {
          case TileType.Dirt:
            AudioClip ac = Resources.Load<AudioClip>("Sounds/scythe");
            AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
            soundCooldown = 3.5f;
            break;
          case TileType.Grass:
            
            break;
          default:
            
            break;
        }
       
    }

    void OnInstalledObjectChanged(InstalledObject obj)
    {
        if(soundCooldown > 0) {
            return;
        }

        AudioClip ac = Resources.Load<AudioClip>("Sounds/wall");
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);

        soundCooldown = 5.5f;
    }

}
