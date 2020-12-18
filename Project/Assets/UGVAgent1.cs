using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class UGVAgent1 : Agent
{
    Rigidbody rBody;
    WheelDriveSkid wheelDriveSkid;
    float limit = 4f;
    float distanceToTargetprev = 4f;
    float intialdistanceToTarget = 0.0f;
    public float  TargetWalkingSpeed =2;


    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        wheelDriveSkid = GetComponent<WheelDriveSkid>();
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
       // if (Mathf.Abs(this.transform.localPosition.x) > limit || Mathf.Abs(this.transform.localPosition.z) > limit || this.transform.localPosition.y < 0)
       // {
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(Random.Range(-4.0f, 4.0f), 0.7f, Random.Range(-4.0f, 4.0f));

            this.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
       // }

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.25f,
                                           Random.value * 8 - 4);

        distanceToTargetprev = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        intialdistanceToTarget = distanceToTargetprev;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(this.transform.InverseTransformDirection(rBody.velocity));
        //sensor.AddObservation(this.transform.localRotation);
        sensor.AddObservation((Vector3.Dot(transform.forward, (Target.position - transform.position).normalized) + 1) * 0.5f);

       // Debug.Log(this.transform.localEulerAngles);
       //Debug.Log(Target.localPosition);
       //Debug.Log(this.transform.localPosition);
       //Debug.Log(rBody.velocity.x);
       //Debug.Log(rBody.velocity.z);



    }
    // Update is called once per frame

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector4 controlSignal = Vector4.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.y = actionBuffers.ContinuousActions[1];
        controlSignal.z = actionBuffers.ContinuousActions[2];
        controlSignal.w = actionBuffers.ContinuousActions[3];
        wheelDriveSkid.torqueFL = controlSignal.x;
        wheelDriveSkid.torqueFR = controlSignal.y;
        wheelDriveSkid.torqueBL = controlSignal.z;
        wheelDriveSkid.torqueBR = controlSignal.w;

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        
        //Debug.Log(dot* matchSpeedReward);
        //Debug.Log((distanceToTargetprev - distanceToTarget) / distanceToTarget);
        //distanceToTargetprev = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        //Debug.Log(matchSpeedReward);
        
        //Debug.Log(lookAtTargetReward);
        //Debug.Log(lookAtTargetReward * ((limit - distanceToTarget) / limit));
        // Reached target
        //Debug.Log(this.transform.localEulerAngles);

        if (distanceToTarget < 1.42f)
        {
            AddReward(10.0f);
            EndEpisode();
        }

        // Fell off platform
        else if (Mathf.Abs(this.transform.localPosition.x) > limit || Mathf.Abs(this.transform.localPosition.z) > limit || this.transform.localPosition.y < 0)
        {
            //SetReward(0f);
            //EndEpisode();
        }
        //AddReward(-0.01f);
    }

    public float GetMatchingVelocityReward(float velocityGoal, float actualVelocity)
    {
        //distance between our actual velocity and goal velocity
        var velDeltaMagnitude = Mathf.Clamp((actualVelocity- velocityGoal), 0, TargetWalkingSpeed);

        //return the value on a declining sigmoid shaped curve that decays from 1 to 0
        //This reward will approach 1 if it matches perfectly and approach zero as it deviates
        return Mathf.Pow(1 - Mathf.Pow(velDeltaMagnitude / TargetWalkingSpeed, 2), 2);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = 0;
        continuousActionsOut[1] = 0;
        continuousActionsOut[2] = 0;
        continuousActionsOut[3] = 0;

        //continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[0] += 0.5f*Input.GetAxis("Horizontal");
        continuousActionsOut[1] += 0.5f * -Input.GetAxis("Horizontal");
        continuousActionsOut[2] += 0.5f * Input.GetAxis("Horizontal");
        continuousActionsOut[3] += 0.5f * -Input.GetAxis("Horizontal");

        continuousActionsOut[0] += 0.5f * Input.GetAxis("Vertical");
        continuousActionsOut[1] += 0.5f * Input.GetAxis("Vertical");
        continuousActionsOut[2] += 0.5f * Input.GetAxis("Vertical");
        continuousActionsOut[3] += 0.5f * Input.GetAxis("Vertical");
    }


    void FixedUpdate()
    {
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        var cubeForward = Target.transform.forward;
        //Debug.Log(cubeForward);
        //Debug.Log(this.transform.forward);
        //var lookAtTargetReward = (Vector3.Dot(cubeForward, this.transform.forward) + 1) * .5F;
        //Debug.Log(lookAtTargetReward);

        //Debug.Log(Mathf.Abs(Vector3.Angle(transform.forward, Target.transform.forward) - 180));

        float dot = (Vector3.Dot(transform.forward, (Target.position - transform.position).normalized) + 1) * 0.5f;

        //Debug.Log(this.transform.localRotation);
        //Debug.Log(dot);
        //AddReward((distanceToTargetprev - distanceToTarget) / distanceToTarget);
        //AddReward(dot* ((distanceToTargetprev - distanceToTarget) / distanceToTarget));

        //Debug.Log(cubeForward * TargetWalkingSpeed);
        var matchSpeedReward = GetMatchingVelocityReward(this.transform.InverseTransformDirection(this.rBody.velocity).z, TargetWalkingSpeed);
        AddReward(dot * (distanceToTargetprev - distanceToTarget) / distanceToTarget);
        //AddReward(dot * Mathf.Clamp((intialdistanceToTarget - distanceToTarget) / intialdistanceToTarget, 0, 1));
        //Debug.Log(GetCumulativeReward());
        //AddReward(-0.1f);
        //AddReward();
        distanceToTargetprev = distanceToTarget;



    }


    void Update()
    {
        //float angle =  Input.GetAxis("Horizontal");
        //float torque = Input.GetAxis("Vertical");
    }
}


