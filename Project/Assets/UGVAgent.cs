using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class UGVAgent : Agent
{
    Rigidbody rBody;
    WheelDrive WheelDrive;
    float limit = 4f;
    float distanceToTargetprev=4f;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        WheelDrive= GetComponent<WheelDrive>();
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        if (Mathf.Abs(this.transform.localPosition.x) > limit || Mathf.Abs(this.transform.localPosition.z) > limit || this.transform.localPosition.y < 0)
        {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.7f, 0);

            this.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        }

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.25f,
                                           Random.value * 8 - 4);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity);
        sensor.AddObservation(this.transform.localEulerAngles);
        //Debug.Log(Target.localPosition);
        //Debug.Log(this.transform.localPosition);
        //Debug.Log(rBody.velocity.x);
        //Debug.Log(rBody.velocity.z);



    }
    // Update is called once per frame

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //// Actions, size = 2
        //Vector3 controlSignal = Vector3.zero;
        //controlSignal.x = actionBuffers.ContinuousActions[0];
        //controlSignal.z = actionBuffers.ContinuousActions[1];
        //WheelDrive.angle= controlSignal.x;
        //WheelDrive.torque = controlSignal.z;

        //// Rewards
        //float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        //var cubeForward = Target.transform.forward;

        //var lookAtTargetReward = (Vector3.Dot(cubeForward, this.transform.forward) + 1) * .5F;
        ////Debug.Log(lookAtTargetReward);

        //AddReward(lookAtTargetReward/10);

        //AddReward((distanceToTargetprev- distanceToTarget)/ distanceToTarget);
        ////Debug.Log((distanceToTargetprev - distanceToTarget) / distanceToTarget);
        //distanceToTargetprev = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        ////Debug.Log(lookAtTargetReward * ((limit - distanceToTarget) / limit));
        //// Reached target


        //if (distanceToTarget < 1.42f)
        //{
        //    SetReward(1.0f);
        //    EndEpisode();
        //}

        //// Fell off platform
        //else if (Mathf.Abs(this.transform.localPosition.x) > limit  || Mathf.Abs(this.transform.localPosition.z) > limit || this.transform.localPosition.y < 0  )
        //{
        //    SetReward(-1f);
        //    EndEpisode();
        //}
        //AddReward(-0.1f);

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        WheelDrive.angle = controlSignal.x;
        WheelDrive.torque = controlSignal.z;

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        var cubeForward = Target.transform.forward;

        var lookAtTargetReward = (Vector3.Dot(cubeForward, this.transform.forward) + 1) * .5F;
        //Debug.Log(lookAtTargetReward);

        AddReward(lookAtTargetReward / 10);

        AddReward((distanceToTargetprev - distanceToTarget) / distanceToTarget);
        Debug.Log((distanceToTargetprev - distanceToTarget) / distanceToTarget);
        distanceToTargetprev = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        //Debug.Log(lookAtTargetReward * ((limit - distanceToTarget) / limit));
        // Reached target


        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        else if (Mathf.Abs(this.transform.localPosition.x) > limit || Mathf.Abs(this.transform.localPosition.z) > limit || this.transform.localPosition.y < 0)
        {
            SetReward(-1f);
            EndEpisode();
        }
        AddReward(-0.1f);



    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }


    void FixedUpdate()
    {

    }


        void Update()
    {
        //float angle =  Input.GetAxis("Horizontal");
        //float torque = Input.GetAxis("Vertical");
    }
}


