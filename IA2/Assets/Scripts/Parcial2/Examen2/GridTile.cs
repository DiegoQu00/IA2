//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen2 --- IDVMI 
//
// Clase GridTile
//
// Clase que tiene cada tile u objeto nodo de nuestro grid, nos sirve para poder detectar
// click izquierdo o derecho sobre cada nodo, para asi definir nuestro punto de inicio y 
// final.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    // Referencia al codigo PathfindingTest
    public PathfindingTest PathFinding;

    // Coordenada x y y
    public int i_x;
    public int i_y;

    // Booleanas que nos avisara si se ha elegido como punto de inicio o final.
    public bool b_initPoint;
    public bool b_endPoint;

    void Start()
    {
        // Encontrar el PathfindingTest en escena
        PathFinding = FindObjectOfType<PathfindingTest>();
    }


    
    // Funcion que detecta cuando el mouse esta sobre el objeto. Necesita un collider.
    private void OnMouseOver()
    {
        
        if (PathFinding.b_InitialPoint == false)
        {
            // Click izquierdo
            if (Input.GetMouseButtonDown(0))
            {
                // Si se elije el mismo punto para inicio y final, se reemplaza.
                if(b_endPoint == true)
                {
                    PathFinding.go_EndPoint = null;
                    PathFinding.b_EndPoint = false;
                    b_endPoint = false;
                }
                // Guardar objeto como punto inicial y activar la booleana
                Debug.Log("Funciona 1");
                PathFinding.go_InitialPoint = gameObject;
                PathFinding.b_InitialPoint = true;
                b_initPoint = true;
                
            }
        }
        


        if (PathFinding.b_EndPoint == false)
        {
            // Click Derecho
            if (Input.GetMouseButtonDown(1))
            {
                // Si se elije el mismo punto para inicio y final, se reemplaza.
                if (b_initPoint == true)
                {
                    PathFinding.go_InitialPoint = null;
                    PathFinding.b_InitialPoint = false;
                    b_initPoint = false;
                }
                // Guardar objeto como punto final y activar la booleana
                Debug.Log("Funciona 2");
                PathFinding.go_EndPoint = gameObject;
                PathFinding.b_EndPoint = true;
                b_endPoint = true;
            }
        }
    }

    // Conseguir Indices de las coordenadas.
    public void GetIndex(int y, int x)
    {
        i_x = x;
        i_y = y;
    }

    // Gizmos para informacion visual
    private void OnDrawGizmos()
    {
        // Poner esfera verde al punto de inicio
        if(b_initPoint==true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 1);
        }
        // Poner esfera rojo al punto final
        if(b_endPoint==true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}
