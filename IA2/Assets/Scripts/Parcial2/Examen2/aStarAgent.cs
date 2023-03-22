//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen2 --- IDVMI 
//
// Clase aStarAgent
//
// Clase encargada de controlar al Agente. Hereda de Steering Behavior para su movimiento.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class aStarAgent : SteeringBehavior
{
    // Booleana para revisar si esta seleccionado o no.
    public bool Selected = false;
    // Color del agente, informacion visual. 
    private SpriteRenderer color;

    // Lista para el camino a seguir.
    public List<Vector3> Path = null;
    // Nodo actual.
    int i_currentWaypoint = 0;

    public PathfindingTest Pathfinding;
    ClassGrid s_Grid;

    public float f_NearArea = .5f;

    void Start()
    {
        color = GetComponent<SpriteRenderer>();
        s_Grid = Pathfinding.myTest;
    }

    // Update is called once per frame
    void Update()
    {
        // Si los puntos de inicio y final estan listos, conseguir el camino.
        if(Pathfinding.b_PathR == true)
        {
            Path = s_Grid.ConvertBacktrackToWorldPos(Pathfinding.Pathfinding_result);
            Pathfinding.b_PathR = false;

        }
            
    }
   

    private void FixedUpdate()
    {
        Vector3 v3SteeringForce = Vector3.zero;

        if (Path != null && Selected == true)
        {
            float f_Distance = (Path[i_currentWaypoint] - transform.position).magnitude;
            Debug.Log("fDistance to Point is: " + f_Distance);

            if (f_NearArea > f_Distance && i_currentWaypoint != Path.Count - 1)
            {

                i_currentWaypoint++;
                i_currentWaypoint = math.min(i_currentWaypoint, Path.Count - 1);
            }

            // Reestructuracion de la condicion
            v3SteeringForce = i_currentWaypoint == Path.Count - 1 ? Seek(Path[i_currentWaypoint]) : Arrive(Path[i_currentWaypoint]);


            r_myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

            //Clamp es para que no exceda la velocidad máxima
            r_myRigidbody.velocity = Vector3.ClampMagnitude(r_myRigidbody.velocity, f_MaxSpeed);


            

        }
    }
    

    // Detecta si el mouse esta sobre el objeto
    private void OnMouseOver()
    {
        // Seleccionar agente
        if (Input.GetMouseButtonDown(0))
        {
            Selected = true;
            Debug.Log("A*Agent is now Selected");
            color.color = Color.cyan;
            r_myRigidbody.isKinematic = false;
        }
        // Deseleccionar agente
        if (Input.GetMouseButtonDown(1))
        {
            Selected = false;
            Debug.Log("A*Agent isnt selected");
            color.color = Color.white;
            r_myRigidbody.isKinematic = true;
        }


    }

    }
