using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    public Rigidbody r_myRigidbody = null;

    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;
    public bool b_UseArrive;
    // Start is called before the first frame update
    void Start()
    {
        r_myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Arrive(Vector3 in_v3v3_TargetPosition)
    {

        Vector3 v3Diff = in_v3v3_TargetPosition - transform.position;
        float fDistance = v3Diff.magnitude;
        float fDesiredMagnitude = f_MaxSpeed;

        if (fDistance < f_ArriveRadius)
        {

            fDesiredMagnitude = Mathf.InverseLerp(0.0f, f_ArriveRadius, fDistance);
        }

        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3SteeringForce = v3DesiredVelocity - r_myRigidbody.velocity;



        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);
        return v3SteeringForce;
    }

    // Funcion Arrive, necesaria para el funcionamiento del Seek.
    private float ArriveFunction(Vector3 in_v3DesiredDirection)
    {
        float fDistance = in_v3DesiredDirection.magnitude;
        float fDesiredMagnitude = f_MaxSpeed;

        if (fDistance < f_ArriveRadius)
        {
            fDesiredMagnitude = Mathf.InverseLerp(0f, f_ArriveRadius, fDistance);
        }

        return fDesiredMagnitude;
    }

    // Funcion Seek, necesaria para el funcionamiento del pursuit. Se mueve hacia el objetivo.
    public Vector3 Seek(Vector3 in_v3v3_TargetPosition)
    {
        //PUNTA-COLA
        Vector3 v3DesiredDirection = in_v3v3_TargetPosition - transform.position;
        float fDesiredMagnitude = f_MaxSpeed;

        if (b_UseArrive)
        {
            fDesiredMagnitude = ArriveFunction(v3DesiredDirection);
        }

        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * f_MaxSpeed;

        Vector3 v3SteeringForce = v3DesiredVelocity - r_myRigidbody.velocity;

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);

        return v3SteeringForce;
    }
}
