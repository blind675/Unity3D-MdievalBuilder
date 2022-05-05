using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour
{
    Dictionary<Character, GameObject> charactersGameObjectsMap;

    Dictionary<string, Sprite> charactersSpritesMap;

    World world
    {
        get { return WorldController.Instance.World; }
    }

    // Start is called before the first frame update
    void Start()
    {
        charactersSpritesMap = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/irishdress");

        foreach (Sprite sprite in sprites)
        {
            charactersSpritesMap[sprite.name] = sprite;
        }

        charactersGameObjectsMap = new Dictionary<Character, GameObject>();
        world.RegisterCharacterCreatedCallback(OnCharacterCreated);

        world.CreateCharacter(world.GetTileAt(world.width / 2, world.height / 2));
    }

    public void OnCharacterCreated(Character character)
    {
        GameObject char_go = new GameObject();

        charactersGameObjectsMap.Add(character, char_go);

        char_go.name = "Character";
        char_go.transform.position = new Vector3(character.currTile.x, character.currTile.y, 0);
        char_go.transform.SetParent(this.transform);

        SpriteRenderer renderer = char_go.AddComponent<SpriteRenderer>();
        renderer.sprite = charactersSpritesMap["irishdress_0"];
        renderer.sortingLayerName = "Characters";
        //renderer.sortingOrder = 1;

        //character.RegisterOnChangedCallback(OnInstalledObjectChanged);
    }
  

    //void OnInstalledObjectChanged(InstalledObject obj)
    //{
    //  // installedObjectGameObjectsMap[obj].GetComponent<SpriteRenderer>().sprite = wallSprite;
    //  if(installedObjectGameObjectsMap.ContainsKey(obj) == false) {
    //    Debug.LogError("InstalledObject "+obj.objectType+" not found in installedObjectGameObjectsMap");
    //    return;
    //  }

    //  GameObject installedObjectGameObject = installedObjectGameObjectsMap[obj];
    //  installedObjectGameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteForInstalledObject(obj);
    //}
}
