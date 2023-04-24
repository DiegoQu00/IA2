using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Basado en el script mostrado en:
// https://docs.unity3d.com/Manual/nav-MoveToClickPoint.html
// Añadí la máscara de colisión para que solo cheque contra el piso y no contra todas las 
// capas.

public class MoveToClickNav : MonoBehaviour
{
    NavMeshAgent _agent = null;
    LayerMask floorMask;
    public Animator animator;
    RaycastHit hit;
    public GameObject Guardia;

    // Start is called before the first frame update
    void Start()
    {
        _agent= GetComponent<NavMeshAgent>();
        floorMask = LayerMask.GetMask("Floor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,
                100.0f, floorMask))
            {
                // Animacion de correr.
                animator.SetBool("IsRunning", true);
                // Le decimos que vaya al punto en el piso que chocó con el rayo de la cámara.
                _agent.destination = hit.point;

            }
            
        }

        float dist = (hit.point - transform.position).magnitude;
        if (dist <= .2f)
        {
            // Desactivar animacion de correr.
            animator.SetBool("IsRunning", false);
        }

        
    }

    private void OnDrawGizmos()
    {
        if (_agent != null && _agent.destination != null)
        { 
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_agent.destination, 1.0f);
        }
    }
}
