using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Hide();
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Car>(out Car car))
        {
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

}
