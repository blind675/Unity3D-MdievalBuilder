using System;
using System.Collections.Generic;
using UnityEngine;

public class JobQueue
{
    Queue<Job> jobQueue;
    Action<Job> cbJobCreated;

    public JobQueue()
    {
        jobQueue = new Queue<Job>();
    }

    public void Enqueue(Job job)
    {
        jobQueue.Enqueue(job);

        if (cbJobCreated != null)
        {
            cbJobCreated(job);
        }
    }

    public void RegisterJobCreatedCallback(Action<Job> callback)
    {
        cbJobCreated += callback;
    }

    public void UnregisterJobCreatedCallback(Action<Job> callback)
    {
        cbJobCreated -= callback;
    }

    // public Job Dequeue()
    // {
    //     if (jobs.Count == 0)
    //     {
    //         return null;
    //     }

    //     Job job = jobs[0];
    //     jobs.RemoveAt(0);
    //     return job;
    // }
}
