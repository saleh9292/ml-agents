using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WheelDriveSkidRotate : MonoBehaviour
{

    private WheelCollider[] m_Wheels;
    public Transform[] m_Legs;

    public float torqueFL = 0;
    public float torqueFR = 0;
    public float torqueBL = 0;
    public float torqueBR = 0;

    public float RotateFL = 0;
    public float RotateFR = 0;
    public float RotateBL = 0;
    public float RotateBR = 0;


    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxTorque = 300f;


    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxRotationspeed = 1f;

    //[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    //public GameObject wheelShape;


    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;


    // Start is called before the first frame update
    void Start()
    {
        m_Wheels = GetComponentsInChildren<WheelCollider>();


        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];

            // Create wheel shapes only when needed.
            //if (wheelShape != null)
            //{
            //   // var ws = Instantiate(wheelShape);
            //   // ws.transform.parent = wheel.transform;



            //}
        }
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

        foreach (WheelCollider wheel in m_Wheels)
        {
            Transform shapeTransform = wheel.transform.GetChild(0);
            Quaternion q;
            Vector3 p;
            wheel.GetWorldPose(out p, out q);


            if (wheel.name == "a0l")
            {

                shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                shapeTransform.position = p;

                wheel.motorTorque = torqueFL * maxTorque;
            }


            if (wheel.name == "a1l")
            {

                shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                shapeTransform.position = p;

                wheel.motorTorque = torqueBL * maxTorque;
            }


            if (wheel.name == "a0r")
            {

                shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                shapeTransform.position = p;

                wheel.motorTorque = torqueFR * maxTorque;
            }


            if (wheel.name == "a1r")
            {

                shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                shapeTransform.position = p;

                wheel.motorTorque = torqueBR * maxTorque;
            }







        }


        m_Legs[0].transform.rotation *= Quaternion.AngleAxis(-RotateFR * Time.fixedDeltaTime, Vector3.forward);
        m_Legs[1].transform.rotation *= Quaternion.AngleAxis(-RotateBR * Time.fixedDeltaTime, Vector3.forward);
        m_Legs[2].transform.rotation *= Quaternion.AngleAxis(RotateFL * Time.fixedDeltaTime, Vector3.forward);
        m_Legs[3].transform.rotation *= Quaternion.AngleAxis(RotateBL * Time.fixedDeltaTime, Vector3.forward);





    }
}

