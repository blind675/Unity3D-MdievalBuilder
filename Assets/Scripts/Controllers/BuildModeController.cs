using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildModeController : MonoBehaviour
{
    bool    isBuildModeObject = false; 
    TileType buildModeTile = TileType.Dirt;
    string  buildModeObjectType;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    public void DoBuild(Tile t) {
      
      if (isBuildModeObject) {
        string jobObjectType = buildModeObjectType;
        if(WorldController.Instance.World.IsInstalledObjectPlacementValid(buildModeObjectType, t) && t.pendingJob == null) {
          Job newJob = new Job(
            t,
            jobObjectType,
            (theJob) => { 
                  WorldController.Instance.World.PlaceInstalledObject(jobObjectType, theJob.tile);
                  t.pendingJob = null;
                }
              );

          t.pendingJob = newJob;
          WorldController.Instance.World.jobQueue.Enqueue(newJob);

        } 
      
      } else {
        t.type = buildModeTile;
      }
    }
    
    public void SetModeDigDirt() {
      isBuildModeObject = false;
      buildModeTile = TileType.Dirt;
    }

    public void SetModeRemove() {
      isBuildModeObject = false;
      buildModeTile = TileType.Empty;
    }

    public void SetModeBuildInstalledObject(string objectType) {
      isBuildModeObject = true;
      buildModeObjectType = objectType;
    }
}
