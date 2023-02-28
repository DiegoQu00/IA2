//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen1 --- IDVMI 
//
// Clase Waypoint
//
// Codigo que se asigna a cada Waypoint generado para conocer su posicion y gestionar todos los waypoints

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    // Referencia al script del agente patrullage
    public Patrullage sPatrullage;

    // Se inicializa al momento de dar play o al generarse el objeto.
    void Start()
    {
        // Se busca el agente patrullage en la escena
        sPatrullage = FindObjectOfType<Patrullage>();

        // Se restablece su posicion en y, ya que inicialmente lo aparece en -10
        transform.position = new Vector3(transform.position.x,transform.position.y,0);

        // Se agrega la posicion del objeto a la lista de waypoints
        sPatrullage.l_Waypoints.Add(transform.position);

    }
    
    //Detecta si se da click izquierdo en el objeto
    private void OnMouseDown()
    {
        // Se remueve de la lista 
        sPatrullage.l_Waypoints.Remove(transform.position);
        // Se destruye
        Destroy(gameObject);
    }
    

    //Si hace trigger con el player
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Si la posicion corresponde a la del waypoint en busqueda actuak
            if (sPatrullage.l_Waypoints[sPatrullage.i_TargetWaypoint] == transform.position)
            {
                // Se manda al siguiente waypoint
                sPatrullage.i_TargetWaypoint = sPatrullage.i_TargetWaypoint + 1;
            }
                
        }
    }


}
