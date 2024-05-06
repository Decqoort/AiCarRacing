using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Barracuda;

public class CarDriverAgent : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;

    private CarController carController;

    private float reward = 40;
    private float startTime = 0;
    private float deltaTime = 10;
    private float distNextCheckpoint = 0;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    private void Start()
    {
        trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
    }

    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    {
        if (e.carTransform == transform)
        {
            AddReward(-reward/2);
        }
    }

    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e)
    {
        Transform nextCheckpointTransform = trackCheckpoints.GetNextCheckpoint(transform).transform;
        distNextCheckpoint = (nextCheckpointTransform.position - transform.position).magnitude;

        Vector3 checkpointForward = nextCheckpointTransform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);

        if (e.carTransform == transform)
        {
            AddReward(directionDot * reward / 2);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.position = spawnPosition.position + new Vector3(Random.Range(-3f, +3f), 0, Random.Range(-3f, +3f));
        transform.forward = spawnPosition.forward;
        trackCheckpoints.ResetCheckpoint(transform);
        carController.StopCompletely();
        startTime = Time.unscaledTime;
        Transform nextCheckpointTransform = trackCheckpoints.GetNextCheckpoint(transform).transform;
        distNextCheckpoint = (nextCheckpointTransform.position - transform.position).magnitude;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Transform nextCheckpointTransform = trackCheckpoints.GetNextCheckpoint(transform).transform;
        Vector3 checkpointForward = nextCheckpointTransform.right;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);

        sensor.AddObservation(directionDot);
        sensor.AddObservation(carController.speed / carController.speedMax);
        sensor.AddObservation(distNextCheckpoint);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = 0f;
        float turnAmount = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0: forwardAmount = 0f; break;
            case 1: forwardAmount = +1f; break;
            case 2: forwardAmount = -1f; break;
        }
        switch (actions.DiscreteActions[1]) 
        { 
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }

        carController.SetInputs(forwardAmount, turnAmount);

        Transform nextCheckpointTransform = trackCheckpoints.GetNextCheckpoint(transform).transform;
        float newDistNextCheckpoint = (nextCheckpointTransform.position - transform.position).magnitude;
        AddReward(reward / 20 * (distNextCheckpoint - newDistNextCheckpoint));
        distNextCheckpoint = newDistNextCheckpoint;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow)) forwardAction = 1;
        if (Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) turnAction = 2;
        if (Input.GetKey(KeyCode.RightArrow)) turnAction = 1;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.2f * reward);
            //EndEpisode();
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.1f * reward);
        }
    }
}
