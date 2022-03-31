using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public GameObject CircleCursorPrefab;
    
    Vector3 currentFramePosition;
    Vector3 lastFramePosition;

    Vector3 dragStartPosition;
    List<GameObject> dragPreviewGameObjects;

    BuildModeController bmc;

    // Start is called before the first frame update
    void Start()
    {
        dragPreviewGameObjects = new List<GameObject>();
        bmc = GameObject.FindObjectOfType<BuildModeController>();
    }

    // Update is called once per frame
    void Update()
    {
        currentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentFramePosition.z = 0;

        // UpdateCursor();
        UpdateDragging();
        UpdateCameraMovement();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateDragging() {

      if(EventSystem.current.IsPointerOverGameObject()){
        return ;
      }

      if(Input.GetMouseButtonDown(0)) {
        dragStartPosition = currentFramePosition;
      }

      int start_x = Mathf.FloorToInt(dragStartPosition.x);
      int end_x = Mathf.FloorToInt(currentFramePosition.x);

      if (end_x < start_x) {
        int tmp = end_x;
        end_x = start_x;
        start_x = tmp;
      }

      int start_y = Mathf.FloorToInt(dragStartPosition.y);
      int end_y = Mathf.FloorToInt(currentFramePosition.y);

      if (end_y < start_y) {
        int tmp = end_y;
        end_y = start_y;
        start_y = tmp;
      }

      while(dragPreviewGameObjects.Count > 0) {
        GameObject go = dragPreviewGameObjects[0];
        dragPreviewGameObjects.RemoveAt(0);
        SimplePool.Despawn(go);
      }

      if(Input.GetMouseButton(0)) {

        for(int x = start_x; x <= end_x; x++) {
          for(int y = start_y; y <= end_y; y++) {
              Tile t = WorldController.Instance.World.GetTileAt(x,y);
              if(t != null) {
                GameObject go = SimplePool.Spawn(CircleCursorPrefab, new Vector3(x,y,0), Quaternion.identity);
                go.transform.SetParent(this.transform, true);
                dragPreviewGameObjects.Add(go);
              }
          }
        }

      }

      if(Input.GetMouseButtonUp(0)) {
  
        for(int x = start_x; x <= end_x; x++) {
          for(int y = start_y; y <= end_y; y++) {
              Tile t = WorldController.Instance.World.GetTileAt(x,y);

              if(t != null) {
                bmc.DoBuild(t);
              }
          }
        }
      }
    }

    void UpdateCameraMovement() {
      
      if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
          Vector3 delta = lastFramePosition - currentFramePosition;
          Camera.main.transform.Translate(delta);
        }

      Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
      Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }
    
}
