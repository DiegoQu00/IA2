//
//              Diego Quintero Martinez  -  IDVMI  -  IA para Videojuegos  -  31/01/2023 
//                      Tarea 1: Agente con el steering behavior "Wander"
//        Recomendado visualizarse en la ventana de escena, no la de play, para apreciar mejor el movimiento del agente.
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public GameObject GO_circle1;                      // Referencia al circulo que guiara nuestro agente
    public GameObject GO_point1;                       // Referencia al punto central del circulo, el centro del circulo y hacia donde empieza el camino del agente. 
    public circle1 s_circle1;                          // Referencia hacia un script que le agregue al circulo guia, solo para obtener mas facilmente su radio con su collider. 
    public Rigidbody myRigidbody = null;               // Rigidbody del agente
    public float f_theta = Mathf.PI/2;                 // Angulo para calcular la posicion del punto de direccion sobre la circunferencia del circulo guia 
    float f_xpoint;                                    // Flotante para el calculo de x del punto de direccion.
    float f_ypoint;                                    // Flotante para el calculo de y del punto de direccion.
    public float f_MaxSpeed = 4f;                      // Flotante velocidad maxima
    public float f_MaxForce = 6f;                      // Flotante fuerza maxima 
    public float f_displaceRange= 0.2f;                // Flotante que nos dara el dezplazamiento del punto de direccion
    private Vector3 v_circletransform = Vector3.zero;  // Vector3 auxiliar para posicionar el circulo guia siempre en frente del agente. 
    


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>(); // Asignar el componente Rigidbody
        if (myRigidbody == null)
        {
            Debug.LogError("No Rigidbody component found for this agent's steering behavior"); // Mensaje de error si no se encuentra Rigidbody en el agente.
            return;
        }
    }


    Vector2 FWander() //Primera version de prueba del Wander, en esta funcion se experimento y se logro el primer funcionamiento, aunque no se podia regular bien la velocidad y fuerza del objeto
    {
        v_circletransform = new Vector3(transform.position.x , transform.position.y + 1.5f, transform.position.z);                           // Posicionamiento del Circulo guia. 
        GO_circle1.transform.position = v_circletransform;                                                                                   // Asignar posicion al circulo guia 
        f_theta += Random.Range(-f_displaceRange, f_displaceRange);                                                                          // Generar un valor random apartir del valor inicial al angulo donde se ubicara el punto de direccion
        f_xpoint = s_circle1.radius * Mathf.Cos(f_theta);                                                                                    // Ubicar la posicion en x del punto de direccion con el radio del circulo guia y el angulo.
        f_ypoint = s_circle1.radius * Mathf.Sin(f_theta);                                                                                    // Ubicar la posicion en y del punto de direccion con el radio del circulo guia y el angulo.
        GO_point1.transform.position = new Vector2(GO_circle1.transform.position.x + f_xpoint, GO_circle1.transform.position.y + f_ypoint);  // Posicionar el punto de direccion
        Vector3 v3SteeringForce = GO_point1.transform.position -  GO_circle1.transform.position;                                             // Primer generacion del Steering Force
        
        
        return v3SteeringForce;
    }

    public Vector3 Wander2() //Segunda version de Wander, ya implementado con base en Seek para el movimiento y correcion de velocidad y fuerza. 
    {

        v_circletransform = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);                           // Posicionamiento del Circulo guia.
        GO_circle1.transform.position = v_circletransform;                                                                                  // Asignar posicion al circulo guia 
        f_theta += Random.Range(-f_displaceRange, f_displaceRange);                                                                         // Generar un valor random apartir del valor inicial al angulo donde se ubicara el punto de direccion
        f_xpoint = s_circle1.radius * Mathf.Cos(f_theta);                                                                                   // Ubicar la posicion en x del punto de direccion con el radio del circulo guia y el angulo.
        f_ypoint = s_circle1.radius * Mathf.Sin(f_theta);                                                                                   // Ubicar la posicion en y del punto de direccion con el radio del circulo guia y el angulo.
        GO_point1.transform.position = new Vector2(GO_circle1.transform.position.x + f_xpoint, GO_circle1.transform.position.y + f_ypoint); // Posicionar el punto de direccion

        
        Vector3 v3DesiredDirection = GO_point1.transform.position - transform.position;             // Punta-Cola para la direccion. 



        
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * f_MaxSpeed;                     // Normalized para que la magnitud de la fuerza nunca sea mayor que la maxSpeed

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;                         // Calculo de SteeringForce

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);                      // ClampMagnitude para la fuerza de la steering force

        return v3SteeringForce;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;                                              // Color rojo en Gizmo
        Gizmos.DrawLine(transform.position, GO_point1.transform.position);     // Linea desde el agente hasta el punto de inicio, centro del circulo guia.

        Gizmos.color = Color.blue;                                             // Color azul en Gizmo
        Gizmos.DrawLine(transform.position, GO_circle1.transform.position);    // Linea desde el agente a nuestro punto de direccion. 

        Gizmos.color= Color.green;                                             // Color verde
        Gizmos.DrawWireSphere(GO_circle1.transform.position, .5f);             // Circulo Guia

    }
    

    void FixedUpdate() //Update, funcion que se repite. 
    {

        //Vector3 v3SteeringForce = FWander();                                           // Llamada a la funcion del primer Wander
        Vector3 v3SteeringForce = Wander2();                                             // Llamada a la funcion del segundo Wander
            

        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);                   //AddForce para el movimiento del agente utilizando la SteeringForce obtenida por Wander

        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, f_MaxSpeed); //Clamp para no exceder la velocidad maxima

    }


}


// CITAS / REFERENCIAS
// The Coding Train  -  5.5 Wander Steering Behavior : The Nature of Code  -  https://www.youtube.com/watch?v=ujsR2vcJlLk&t=450s&ab_channel=TheCodingTrain
// Craig W. Reynolds  -  Steering Behaviors For Autonomous Characters  -  http://www.red3d.com/cwr/papers/1999/gdc99steer.pdf
// Unity Documentation  -  https://docs.unity.com/
