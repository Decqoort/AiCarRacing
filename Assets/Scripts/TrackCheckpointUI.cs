using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpointUI : MonoBehaviour
{
    [SerializeField] private TrackCheckpoints TrackCheckpoints;

    private void Start()
    {
        TrackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        TrackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;

        Hide();
    }

    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, EventArgs e)
    {
        Show();   
    }

    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
