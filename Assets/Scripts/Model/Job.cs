using System;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public Tile tile {get; protected set; }
    public string jobObjectType {get; protected set; }

    float jobTime;

    Action<Job> cbJobComplete;
    Action<Job> cbJobCancel;

    public Job(Tile tile, string jobObjectType, Action<Job> cbJobComplete,  float jobTime = 1f) {
        this.tile = tile;
        this.cbJobComplete += cbJobComplete;
        this.jobObjectType = jobObjectType;
        this.jobTime = jobTime;
    }

    public void RegisterJobCompleteCallback(Action<Job> cb) {
        cbJobComplete += cb;
    }

    public void RegisterJobCancelCallback(Action<Job> cb) {
        cbJobCancel += cb;
    }

    public void UnregisterJobCompleteCallback(Action<Job> cb) {
        cbJobComplete -= cb;
    }

    public void UnregisterJobCancelCallback(Action<Job> cb) {
        cbJobCancel -= cb;
    }

    public void DoWork(float workTime) {
        jobTime -= workTime;

        if (jobTime <= 0)  {
            if (cbJobComplete != null) {
                cbJobComplete(this);
            }
        }
    }

    public void CancelJob() {
        if (cbJobCancel != null) {
            cbJobCancel(this);
        }
    }
}
