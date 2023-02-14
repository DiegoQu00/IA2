// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen1 --- IDVMI 
//
// Clase Infiltrador
//
// Agente que se movera hacia donde se de click, podra ser detectado por el guardia si entra en su campo de vision. 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infiltrador : MonoBehaviour
{
    // Declaracion de variables

    public Rigidbody myRigidbody;

    // Variables de movimiento
    [Header("Movement")]
    public float fMaxSpeed = 4f;
    public float fArriveRadius = 2f;
    public float fMaxForce = 6f;
    public Vector3 TargetPosition;

    // Objetivo del Agente
    enum SteeringTarget { mouse }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.mouse;

    // Se inicializa al momento de dar play o al generarse el objeto.
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    //Se actualiza cada frame
    void Update()
    {
        switch (currentTarget)
        {
            // Se asigna el objetivo a la posicion del mouse cada vez que se de click
            case SteeringTarget.mouse:
                if (Input.GetMouseButtonDown(0))
                {
                    TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                break;   
        } 
    }


    // Corre a un rate estable, a diferencia del update que corre cada frame variando entre una y otra computadora.
    private void FixedUpdate()
    {

        TargetPosition.z = 0.0f;    

        Vector3 v3SteeringForce = Vector3.zero;

            

        // Se hace el arrive hacia el ultimo click del mouse. 
        v3SteeringForce = Arrive(TargetPosition);
                   //DrawGizmos

        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

        //Clamp es para que no exceda la velocidad máxima
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);

    }

    // Funcion de movimiento Arrive, se mueve hacia un punto objetivo. Nos devuelve la SteeringForce necesaria para el movimiento, de acuerdo a el objetivo deseado.
    Vector3 Arrive(Vector3 in_v3TargetPosition)
    {

        Vector3 v3Diff = in_v3TargetPosition - transform.position;
        float fDistance = v3Diff.magnitude;
        float fDesiredMagnitude = fMaxSpeed;

        if (fDistance < fArriveRadius)
        {

            fDesiredMagnitude = Mathf.InverseLerp(0.0f, fArriveRadius, fDistance);
        }


        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;


        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fMaxForce);
        return v3SteeringForce;
    }

    

}
