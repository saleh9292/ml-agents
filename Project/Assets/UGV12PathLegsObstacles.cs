using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgentsExamples;
public class UGV12PathLegsObstacles : Agent
{
    Rigidbody rBody;
    WheelDriveSkidRotate wheelDriveSkid;
    float limit = 4f;
    float distanceToTargetprev = 4f;

    float currentD = 0f;
    float minD = 0f;
    float destReward = 0f;

    float intialdistanceToTarget = 0.0f;
    public float TargetWalkingSpeed = 1;

    OrientationCubeController m_OrientationCube;
    DirectionIndicator m_DirectionIndicator;

    public PathObstaclesGenrator pathGenrator;
    float c;
    // bool a, b, c = false;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        wheelDriveSkid = GetComponent<WheelDriveSkidRotate>();
        m_OrientationCube = GetComponentInChildren<OrientationCubeController>();
        m_DirectionIndicator = GetComponentInChildren<DirectionIndicator>();
        //mazespawner= GetComponent<mazespawner>();

        pathGenrator.CreatePath();
    }

    public Transform Target;
    //public Transform Obstacle;
    public override void OnEpisodeBegin()
    {
        // if (Mathf.Abs(this.transform.localPosition.x) > limit || Mathf.Abs(this.transform.localPosition.z) > limit || this.transform.localPosition.y < 0)
        // {
        // If the Agent fell, zero its momentum
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 1.1f, 0f);

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        // }


        wheelDriveSkid.m_Legs[0].localRotation = Quaternion.Euler(0, 180, Random.Range(0.0f, 360.0f));
        wheelDriveSkid.m_Legs[1].localRotation = Quaternion.Euler(0, 180, Random.Range(0.0f, 360.0f));
        wheelDriveSkid.m_Legs[2].localRotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
        wheelDriveSkid.m_Legs[3].localRotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));


        pathGenrator.DestroyPath();

        pathGenrator.CreatePath();

        // Move the target to a new spot
        //Target.localPosition = new Vector3(Random.Range(2.0f, 9f), 0.5f, Random.Range(2.0f, 9.0f));
        //Target.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);

        //Obstacle.localPosition = new Vector3(Random.Range(-5.0f, 5f), 0.5f, Random.Range(-1.0f, 1.0f));
        //Obstacle.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        //RUN maze genreation 

        distanceToTargetprev = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        intialdistanceToTarget = distanceToTargetprev;
        minD = distanceToTargetprev;

        UpdateOrientationObjects();
    }


    public override void CollectObservations(VectorSensor sensor)
    {

        var cubeForward = m_OrientationCube.transform.forward;

        var avgVel = GetAvgVelocity();
        var velGoal = cubeForward * TargetWalkingSpeed;
        //current ragdoll velocity. normalized
        sensor.AddObservation(Vector3.Distance(velGoal, avgVel));          //3 
        //avg body vel relative to cube
        sensor.AddObservation(m_OrientationCube.transform.InverseTransformDirection(avgVel));
        //vel goal relative to cube
        sensor.AddObservation(m_OrientationCube.transform.InverseTransformDirection(velGoal));
        //rotation delta
        sensor.AddObservation(Quaternion.FromToRotation(transform.forward, cubeForward));

        sensor.AddObservation(m_OrientationCube.transform.InverseTransformPoint(Target.transform.position));

        //legs angles
        sensor.AddObservation(wheelDriveSkid.m_Legs[0].localRotation.eulerAngles.z);
        sensor.AddObservation(wheelDriveSkid.m_Legs[1].localRotation.eulerAngles.z);
        sensor.AddObservation(wheelDriveSkid.m_Legs[2].localRotation.eulerAngles.z);
        sensor.AddObservation(wheelDriveSkid.m_Legs[3].localRotation.eulerAngles.z);

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
        //// Actions, size = 2
        //Vector4 controlSignal = Vector4.zero;
        //controlSignal.x = actionBuffers.ContinuousActions[0];
        //controlSignal.y = actionBuffers.ContinuousActions[1];
        //controlSignal.z = actionBuffers.ContinuousActions[2];
        //controlSignal.w = actionBuffers.ContinuousActions[3];
        //wheelDriveSkid.torqueFL = controlSignal.x;
        //wheelDriveSkid.torqueFR = controlSignal.y;
        //wheelDriveSkid.torqueBL = controlSignal.z;
        //wheelDriveSkid.torqueBR = controlSignal.w;
        //wheelDriveSkid.RotateBL = actionBuffers.ContinuousActions[4];
        //wheelDriveSkid.RotateBR = actionBuffers.ContinuousActions[5];
        //wheelDriveSkid.RotateFL = actionBuffers.ContinuousActions[6];
        //wheelDriveSkid.RotateFR = actionBuffers.ContinuousActions[7];




        int whheel1 = actionBuffers.DiscreteActions[0];
        int whheel2 = actionBuffers.DiscreteActions[1];
        int whheel3 = actionBuffers.DiscreteActions[2];
        int whheel4 = actionBuffers.DiscreteActions[3];
        int leg1 = actionBuffers.DiscreteActions[4];
        int leg2 = actionBuffers.DiscreteActions[5];
        int leg3 = actionBuffers.DiscreteActions[6];
        int leg4 = actionBuffers.DiscreteActions[7];


        if (whheel1 == 1) { wheelDriveSkid.torqueFL = 1; }
        if (whheel1 == 2) { wheelDriveSkid.torqueFL = -1; }
        if (whheel1 == 3) { wheelDriveSkid.torqueFL = 0; }

        if (whheel2 == 1) { wheelDriveSkid.torqueFR = 1; }
        if (whheel2 == 2) { wheelDriveSkid.torqueFR = -1; }
        if (whheel2 == 3) { wheelDriveSkid.torqueFR = 0; }

        if (whheel3 == 1) { wheelDriveSkid.torqueBL = 1; }
        if (whheel3 == 2) { wheelDriveSkid.torqueBL = -1; }
        if (whheel3 == 3) { wheelDriveSkid.torqueBL = 0; }


        if (whheel4 == 1) { wheelDriveSkid.torqueBR = 1; }
        if (whheel4 == 2) { wheelDriveSkid.torqueBR = -1; }
        if (whheel4 == 3) { wheelDriveSkid.torqueBR = 0; }



        if (leg1 == 1) { wheelDriveSkid.RotateFL = 1; }
        if (leg1 == 2) { wheelDriveSkid.RotateFL = -1; }
        if (leg1 == 3) { wheelDriveSkid.RotateFL = 0; }

        if (leg2 == 1) { wheelDriveSkid.RotateFR = 1; }
        if (leg2 == 2) { wheelDriveSkid.RotateFR = -1; }
        if (leg2 == 3) { wheelDriveSkid.RotateFR = 0; }

        if (leg3 == 1) { wheelDriveSkid.RotateBL = 1; }
        if (leg3 == 2) { wheelDriveSkid.RotateBL = -1; }
        if (leg3 == 3) { wheelDriveSkid.RotateBL = 0; }


        if (leg4 == 1) { wheelDriveSkid.RotateBR = 1; }
        if (leg4 == 2) { wheelDriveSkid.RotateBR = -1; }
        if (leg4 == 3) { wheelDriveSkid.RotateBR = 0; }







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
            AddReward(1f);
            //EndEpisode();
            //Target.localPosition = new Vector3(Random.Range(2.0f, 9f), 0.5f, Random.Range(2.0f, 9.0f));
            //Target.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);

            bool s = pathGenrator.NextTarget();

            if (s == true)
            {
                AddReward(10f);
                EndEpisode();

            }



            //this.transform.localPosition = new Vector3(0, 0.7f, 0);

            //this.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);

            //Obstacle.localPosition = new Vector3(Random.Range(-5.0f, 5f), 0.5f, Random.Range(-1.0f, 1.0f));
            //Obstacle.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);

            distanceToTargetprev = Vector3.Distance(this.transform.localPosition, Target.localPosition);
            intialdistanceToTarget = distanceToTargetprev;
            minD = distanceToTargetprev;
            // UpdateOrientationObjects();
            //EndEpisode();

            //genrate new maze
            //mazespawner.DestroyMaze();

            //mazespawner.CreateMaze();

        }


        if (transform.localPosition.y < 0)
        {

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
        //var continuousActionsOut = actionsOut.ContinuousActions;
        //continuousActionsOut[0] = 0;
        //continuousActionsOut[1] = 0;
        //continuousActionsOut[2] = 0;
        //continuousActionsOut[3] = 0;

        //continuousActionsOut[0] += Input.GetAxis("Horizontal");
        //continuousActionsOut[0] += Input.GetAxis("Horizontal");
        //continuousActionsOut[1] += -Input.GetAxis("Horizontal");
        //continuousActionsOut[2] += Input.GetAxis("Horizontal");
        //continuousActionsOut[3] += -Input.GetAxis("Horizontal");

        //continuousActionsOut[0] += Input.GetAxis("Vertical");
        //continuousActionsOut[1] += Input.GetAxis("Vertical");
        //continuousActionsOut[2] += Input.GetAxis("Vertical");
        //continuousActionsOut[3] += Input.GetAxis("Vertical");
        //if (Input.GetKey(KeyCode.A))
        //{
        //    continuousActionsOut[4] = 1;

        //}


        //if (Input.GetKey(KeyCode.S))
        //{

        //    continuousActionsOut[5] = 1;
        //}

        //if (Input.GetKey(KeyCode.D))
        //{

        //    continuousActionsOut[6] = 1;
        //}


        //if (Input.GetKey(KeyCode.F))
        //{

        //    continuousActionsOut[7] = 1;
        //}

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
        //AddReward(matchSpeedReward * lookAtTargetReward * 0.1f);
        currentD = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if (currentD < minD - 0.2)
        {
            destReward = (minD - currentD) / intialdistanceToTarget;
            AddReward(destReward);


            minD = currentD;
            c = 0;

        }
        else
        {
            c += Time.deltaTime;



        }
        //Debug.Log(c);
        //Debug.Log(GetCumulativeReward());
        //c += Time.deltaTime;
        if (c >= 15)
        {
            //SetReward(0);
            EndEpisode();
            c = 0;



        }

        //Debug.Log("Time: "+c +"MinD"+ minD +"currentD" +currentD);

    }


    void Update()
    {
        //float angle =  Input.GetAxis("Horizontal");
        //float torque = Input.GetAxis("Vertical");
    }
}

