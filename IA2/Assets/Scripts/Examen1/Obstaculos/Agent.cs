//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen1 --- IDVMI 
//
// Clase AgenteEsquivaObstaculos
//
// Agente que se movera hacia donde se de click, si en su camino encuentra algun obstaculo tiene cambiar su ruta para no chocar, pero mantener su objetivo.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Agent : MonoBehaviour
{
    // Declaracion de variables

    // Variables de componentes
    [Header("Components")]
    public GameObject go_Obstaculo;
    public Rigidbody myRigidbody;

    // Variables de movimiento
    [Header("Movement")]
    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;
    public Vector3 v3_TargetPosition;
    public Vector3 v3_SteeringForce;

    private bool b_fleeing = false;

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
                    v3_TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                break;


        }


    }

    // Si  detecta algun obstaculo en su alrededor
    private void OnTriggerEnter(Collider other)
    {
        // Activa Flee();
        b_fleeing = true;
        // Guarda el objeto, para conocer su posicion.
        go_Obstaculo = other.gameObject;
        

    }

    // Si el objeto sale de su alrededor
    private void OnTriggerExit(Collider other)
    {
        //desactiva el Flee();
        b_fleeing = false;
    }

    // Corre a un rate estable, a diferencia del update que corre cada frame variando entre una y otra computadora.
    private void FixedUpdate()
    {

        v3_TargetPosition.z = 0.0f;    //Le pone la z de la camara

        v3_SteeringForce = Vector3.zero;

        
        // Movimiento usual hacia el click del mouse
        v3_SteeringForce = Arrive(v3_TargetPosition);

        // Si se detecta un objeto, sumar el Flee al movimiento actual
        if(b_fleeing == true)
            v3_SteeringForce += Flee(go_Obstaculo.transform.position);
        
            



        myRigidbody.AddForce(v3_SteeringForce, ForceMode.Acceleration);  

        //Clamp es para que no exceda la velocidad máxima
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, f_MaxSpeed);

    }

    // Funcion de movimiento Arrive, se mueve hacia un punto objetivo. Nos devuelve la SteeringForce necesaria para el movimiento, de acuerdo a el objetivo deseado.
    Vector3 Arrive(Vector3 in_v3v3_TargetPosition)
    {

        Vector3 v3Diff = in_v3v3_TargetPosition - transform.position;
        float fDistance = v3Diff.magnitude;
        float fDesiredMagnitude = f_MaxSpeed;

        if (fDistance < f_ArriveRadius)
        {
            
            fDesiredMagnitude = Mathf.InverseLerp(0.0f, f_ArriveRadius, fDistance);
        }


        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3_SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        

        v3_SteeringForce = Vector3.ClampMagnitude(v3_SteeringForce, f_MaxForce);
        return v3_SteeringForce;
    }

    public Vector3 Flee(Vector3 in_v3v3_TargetPosition)
    {
        //PUNTA-COLA
        Vector3 v3DesiredDirection = transform.position - in_v3v3_TargetPosition;

        //Normalized para que la magnitud de la fuerza nunca sea mayor que la maxSpeed
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * f_MaxSpeed;

        Vector3 v3_SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        v3_SteeringForce = Vector3.ClampMagnitude(v3_SteeringForce, f_MaxForce);

        return v3_SteeringForce;
    }

    //Gizmo para mostrarlos en pantalla
    private void OnDrawGizmos()
    {
        //Gizmo para mostrar la conexion entre el punto a llegar y el agente
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, v3_TargetPosition);
    }
}
