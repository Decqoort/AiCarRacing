using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{

    public class CarCheckpointEventArgs : EventArgs
    {
        public Transform carTransform;
    }

    public event EventHandler<CarCheckpointEventArgs> OnCarCorrectCheckpoint;
    public event EventHandler<CarCheckpointEventArgs> OnCarWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList; 
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform) 
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }
        nextCheckpointSingleIndexList = new List<int>();

        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];

        CarCheckpointEventArgs carCheckpointEventArgs = new CarCheckpointEventArgs();
        carCheckpointEventArgs.carTransform = carTransform;

        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            //  Correct checkpoint
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            //correctCheckpointSingle.Hide();

            do
            {
                nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            }
            while (!checkpointSingleList[nextCheckpointSingleIndex].gameObject.activeSelf);

            if (nextCheckpointSingleIndex == 0)
            {
                if(carTransform.TryGetComponent<Car>(out Car car))
                {
                    car.isEndLap = true;
                }
            }

            if (nextCheckpointSingleIndex == 1)
            {
                if (carTransform.TryGetComponent<Car>(out Car car))
                {
                    if (car.isEndLap)
                    {
                        car.currentLap++;
                        car.isEndLap = false;
                    }
                }
            }

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = nextCheckpointSingleIndex;
            OnCarCorrectCheckpoint?.Invoke(this, carCheckpointEventArgs);
            //Debug.Log(checkpointSingleList[nextCheckpointSingleIndex]);
        }
        else
        {
            //  Wrong checkpoint
            OnCarWrongCheckpoint?.Invoke(this, carCheckpointEventArgs);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            //correctCheckpointSingle.Show();
        }
    }

    public void ResetCheckpoint(Transform transform)
    {
        nextCheckpointSingleIndexList[carTransformList.IndexOf(transform)] = 0;
    }

    public CheckpointSingle GetNextCheckpoint(Transform transform)
    {
        return checkpointSingleList[nextCheckpointSingleIndexList[carTransformList.IndexOf(transform)]];
    }
}
