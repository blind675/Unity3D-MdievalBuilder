using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSpriteController : MonoBehaviour
{
    InstalledObjectSpriteController installedObjectSpriteController;
    Dictionary<Job, GameObject> jobGameObjectMap;

    // Start is called before the first frame update
    void Start()
    {
        installedObjectSpriteController = GameObject.FindObjectOfType<InstalledObjectSpriteController>();
        jobGameObjectMap = new Dictionary<Job, GameObject>();

        WorldController.Instance.World.jobQueue.RegisterJobCreatedCallback(OnJobCreated);
    }

    void OnJobCreated(Job job)
    {
        GameObject job_go = new GameObject();

        jobGameObjectMap.Add(job, job_go);

        job_go.name = "Job_" +job.jobObjectType + "_" + job.tile.x + "_" + job.tile.y;
        job_go.transform.position = new Vector3(job.tile.x, job.tile.y, 0);
        job_go.transform.SetParent(this.transform);

        SpriteRenderer renderer = job_go.AddComponent<SpriteRenderer>();
        renderer.sprite = installedObjectSpriteController.GetSpriteForInstalledObject(job.jobObjectType);
        renderer.color = new Color(1f,1f,1f,0.5f);
        renderer.sortingLayerName = "Jobs";

        job.RegisterJobCompleteCallback(OnJobEnded);
        job.RegisterJobCancelCallback(OnJobEnded);
    }

    void OnJobEnded(Job job)
    { 
        GameObject job_go = jobGameObjectMap[job];
        job.UnregisterJobCompleteCallback(OnJobEnded);
        job.UnregisterJobCancelCallback(OnJobEnded);

        Destroy(job_go);
    }
}
