using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;
public class UGVAgent3 : Agent
{
    Rigidbody rBody;
    WheelDriveSkid wheelDriveSkid;
    float limit = 4f;
    float distanceToTargetprev = 4f;
    float intialdistanceToTarget = 0.0f;
    public float TargetWalkingSpeed = 2;

    OrientationCubeController m_OrientationCube;
    DirectionIndicator m_DirectionIndicator;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        wheelDriveSkid = GetComponent<WheelDriveSkid>();
        m_OrientationCube = GetComponentInChildren<OrientationCubeController>();
        m_DirectionIndicator = GetComponentInChildren<DirectionIndicator>();
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

        var cubeForward = m_OrientationCube.transform.forward;

        var avgVel = GetAvgVelocity();
        var velGoal = cubeForward * TargetWalkingSpeed;
        //current ragdoll velocity. normalized
        sensor.AddObservation(Vector3.Distance(velGoal, avgVel));
        //avg body vel relative to cube
        sensor.AddObservation(m_OrientationCube.transform.InverseTransformDirection(avgVel));
        //vel goal relative to cube
        sensor.AddObservation(m_OrientationCube.transform.InverseTransformDirection(velGoal));
        //rotation delta
        sensor.AddObservation(Quaternion.FromToRotation(transform.forward, cubeForward));

        sensor.AddObservation(m_OrientationCube.transform.InverseTransformPoint(Target.transform.position));



    }
    // Update is called once per frame


    Vector3 GetAvgVelocity()
    {
        Vector3 velSum = Vector3.zero;
        Vector3 avgVel = Vector3.zero;
        int numOfRB = 1;
        //ALL RBS
        //int numOfRB = 0;
        //foreach (var item in m_JdController.bodyPartsList)
        //{
        //    numOfRB++;
        //    velSum += item.rb.velocity;
        //}

        avgVel = rBody.velocity / numOfRB;
        return avgVel;
    }
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
            AddReward(1.0f);
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

    public float GetMatchingVelocityReward(Vector3 velocityGoal, Vector3 actualVelocity)
    {
        //distance between our actual velocity and goal velocity
        var velDeltaMagnitude = Mathf.Clamp(Vector3.Distance(actualVelocity, velocityGoal), 0, TargetWalkingSpeed);

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
        continuousActionsOut[0] += Input.GetAxis("Horizontal");
        continuousActionsOut[1] += -Input.GetAxis("Horizontal");
        continuousActionsOut[2] += Input.GetAxis("Horizontal");
        continuousActionsOut[3] += -Input.GetAxis("Horizontal");

        continuousActionsOut[0] += Input.GetAxis("Vertical");
        continuousActionsOut[1] += Input.GetAxis("Vertical");
        continuousActionsOut[2] += Input.GetAxis("Vertical");
        continuousActionsOut[3] += Input.GetAxis("Vertical");


       //Debug.Log(transform.forward);
        //Debug.Log(Target.transform.forward);
    }
    void UpdateOrientationObjects()
    {
        m_OrientationCube.UpdateOrientation(transform, Target);
        if (m_DirectionIndicator)
        {
            m_DirectionIndicator.MatchOrientation(m_OrientationCube.transform);
        }
    }

    void FixedUpdate()
    {

        UpdateOrientationObjects();


        var cubeForward = m_OrientationCube.transform.forward;

        // Set reward for this step according to mixture of the following elements.
        // a. Match target speed
        //This reward will approach 1 if it matches perfectly and approach zero as it deviates
        var matchSpeedReward = GetMatchingVelocityReward(cubeForward * TargetWalkingSpeed, GetAvgVelocity());

        // b. Rotation alignment with target direction.
        //This reward will approach 1 if it faces the target direction perfectly and approach zero as it deviates

        var lookAtTargetReward = (Vector3.Dot(cubeForward, transform.forward) + 1) * .5F;
        //Debug.Log(lookAtTargetReward);
        AddReward(matchSpeedReward * lookAtTargetReward);


    }


    void Update()
    {
        //float angle =  Input.GetAxis("Horizontal");
        //float torque = Input.GetAxis("Vertical");
    }
}


