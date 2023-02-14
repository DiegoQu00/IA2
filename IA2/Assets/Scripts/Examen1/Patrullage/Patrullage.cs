//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen1 --- IDVMI 
//
// Clase Patrullage
//
// Agente que se mueve entre Waypoints que se pueden agregar o quitar ingame, se inicia la escena con 2 waypoints aleatorios por defecto.  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullage : MonoBehaviour
{
    // Declaracion de variables

    // Variables de componentes
    [Header("Components")]
    public GameObject go_Waypoint;
    public Rigidbody myRigidbody;

    // Variables de movimiento
    [Header("Movement")]
    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;
    public int i_TargetWaypoint=0;
    public int i_length;
    public Vector3 v3_TargetPosition;
    //Arreglo Dinamico(lista) para guardar los waypoints
    public List<Vector3> l_Waypoints = new List<Vector3>();

    // Objetivo del Agente
    enum SteeringTarget { Waypoint }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.Waypoint;

    // Se inicializa al momento de dar play o al generarse el objeto.
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

        // Generacion aleatoria de los 2 primeros puntos.
        for(int i=0;i<2;i++)
            Instantiate(go_Waypoint, new Vector3(Random.Range(-6,6), Random.Range(-3, 3),0), Quaternion.identity);

    }

    //Se actualiza cada frame
    void Update()
    {
        // Obtener cuantos objetos tenemos en nuestra lista.
        i_length = l_Waypoints.Count;

        // Si se da click derecho, se agrega un waypoint.
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(go_Waypoint, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }

        // Si no hay waypoints disponibles, el agente para.
        if (i_length == 0)
        {
            f_MaxSpeed = 0;
        }

        // Velocidad normal
        else
            f_MaxSpeed = 6;

        switch (currentTarget)
        {
            //Llamada a la funcion patrullage, que gestiona los waypoints
            case SteeringTarget.Waypoint:
                        Patrullagef();

                    break;


        }


    }



    private void FixedUpdate()
    {

        v3_TargetPosition.z = 0.0f;    

        Vector3 v3SteeringForce = Vector3.zero;




        v3SteeringForce = Arrive(v3_TargetPosition);
        //DrawGizmos

        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

        //Clamp es para que no exceda la velocidad máxima
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, f_MaxSpeed);

    }

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

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;


        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);
        return v3SteeringForce;
    }

    // Funcion Patrullage, nos resetea si llegamos al ultimo waypoint y asigna el valor a targetPosition
    private void Patrullagef()
    {

        if(i_TargetWaypoint == i_length)
            i_TargetWaypoint = 0;
    
        v3_TargetPosition = l_Waypoints[i_TargetWaypoint];
    }
}
